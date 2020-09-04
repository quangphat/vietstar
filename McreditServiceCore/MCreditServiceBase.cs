using AutoMapper;
using VietStar.Entities.Mcredit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using HttpService;
using VietStar.Entities.Commons;
using static VietStar.Entities.Commons.Enums;
using System.Linq;
using VietStar.Entities.Mcredit;
using System.IO;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities;
using Microsoft.Extensions.Options;

namespace McreditServiceCore
{
    public abstract class MCreditServiceBase
    {
        protected readonly HttpClient _httpClient;
        protected HttpRequestMessage _requestMessage;
        protected readonly IMCreditRepository _rpMcredit;
        protected readonly ILogRepository _rpLog;
        protected int _userId;
        protected string _userToken;
        protected readonly IMapper _mapper;
        public readonly CurrentProcess _process;
        protected readonly McreditApi _mcApiconfig;
        protected static string _baseUrl;
        protected static string _contentType = "application/json";
        protected MCreditServiceBase(IMCreditRepository mCreditRepository,
            ILogRepository logRepository,
            IOptions<McreditApi> option,
            CurrentProcess process)
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
            _mcApiconfig = option.Value;
            _requestMessage.Headers.Add("xdncode", _mcApiconfig.XDNCode);
            _baseUrl = _mcApiconfig.BaseUrl;
            _userToken = string.Empty;
            _rpMcredit = mCreditRepository;
            _rpLog = logRepository;
            _process = process;
        }
        public async Task<BaseResponse<string>> AuthenByUserId(int userId, int[] tableToUpdateIds = null)
        {
            if (userId <= 0)
                return ToResponse<string>(null, "Vui lòng nhập userId");
            var authen = await Authen();
            if (authen == null || authen.Obj == null || string.IsNullOrWhiteSpace(authen.Obj.Token))
                return ToResponse<string>(null, "Không thể authen");
            string token = authen.Obj.Token;

            await _rpMcredit.InsertUserTokenAsync(new MCreditUserToken { Token = token, UserId = userId });

            if (tableToUpdateIds == null || !tableToUpdateIds.Any())
                return ToResponse(token);

            if (tableToUpdateIds.Contains((int)MCTableType.MCreditProduct) && authen.Products != null && authen.Products.Any())
            {
                await _rpMcredit.DeleteMCTableDatasAsync((int)MCTableType.MCreditProduct);
                await _rpMcredit.InsertProductsAsync(authen.Products);
                await _rpLog.InsertLog("mcredit-update-product", authen.Products.Dump());
            }

            if (tableToUpdateIds.Contains((int)MCTableType.MCreditCity) && authen.Cities != null && authen.Cities.Any())
            {
                await _rpMcredit.DeleteMCTableDatasAsync((int)MCTableType.MCreditCity);
                await _rpMcredit.InsertCitiesAsync(authen.Cities);
                await _rpLog.InsertLog("mcredit-update-cities", authen.Cities.Dump());
            }

            if (tableToUpdateIds.Contains((int)MCTableType.MCreditLoanPeriod) && authen.LoanPeriods != null && authen.LoanPeriods.Any())
            {
                await _rpMcredit.DeleteMCTableDatasAsync((int)MCTableType.MCreditLoanPeriod);
                await _rpMcredit.InsertLoanPeriodsAsync(authen.LoanPeriods);
                await _rpLog.InsertLog("mcredit-update-loanperiod", authen.LoanPeriods.Dump());
            }

            if (tableToUpdateIds.Contains((int)MCTableType.MCreditlocations) && authen.Locations != null && authen.Locations.Any())
            {
                await _rpMcredit.DeleteMCTableDatasAsync((int)MCTableType.MCreditlocations);
                await _rpMcredit.InsertLocationsAsync(authen.Locations);
                await _rpLog.InsertLog("mcredit-update-locations", authen.Locations.Dump());
            }

            if (tableToUpdateIds.Contains((int)MCTableType.MCreditProfileStatus) && authen.ProfileStatus != null && authen.ProfileStatus.Any())
            {
                var list = new List<OptionSimple>();
                foreach (var item in authen.ProfileStatus)
                {
                    list.Add(new OptionSimple { Code = item.Key, Name = item.Value });
                }
                await _rpMcredit.DeleteMCTableDatasAsync((int)MCTableType.MCreditProfileStatus);
                await _rpMcredit.InsertProfileStatusAsync(list);
            }

            return ToResponse(token);
        }
        protected async Task<AuthenResponse> Authen()
        {
            var result = await _httpClient.PostAsync<AuthenResponse>(_requestMessage, _baseUrl, _mcApiconfig.AuthenApi, _contentType, null, new AuthenRequestModel
            {
                UserName = _mcApiconfig.UserName,
                UserPass = _mcApiconfig.Password,
                token = _mcApiconfig.AuthenToken
            });
            return result.Data;
        }
        protected async Task<BaseResponse<T>> BeforeSendRequest<T, TInput>(string apiPath, TInput model)
            where T : MCResponseModelBase
            where TInput : MCreditRequestModelBase
        {
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _mcApiconfig.XDNCode);
            model.token = await GetUserToken(_process.User.Id);
            await _rpLog.InsertLog($"mcredit-{apiPath}", model.Dump());
            var result = await _httpClient.PostAsync<T>(_requestMessage, _baseUrl, apiPath, _contentType, null, model, rpLog: _rpLog);
            if (result != null)
            {
                await _rpLog.InsertLog($"mcredit-{apiPath}", result.Dump());
            }
            if (result == null || result.Data == null)
                return ToResponse<T>(null, Errors.NoData);
            if (result.Data is T)
            {
                var data = (T)result.Data;
                data.message = JsonConvert.SerializeObject(data.msg);

                return ToResponse<T>(data, data.status =="success" ?null : data.msg.ToString());
            }
            return ToResponse<T>(result.Data);
        }
        protected async Task<BaseResponse<T>> BeforeSendRequestUploadFile<T, TInput>(string apiPath, Dictionary<string, object> postParameters)
           where T : MCResponseModelBase
        {
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _mcApiconfig.XDNCode);
            var token = await GetUserToken(_process.User.Id);
            try
            {
                string requestURL = $"{_baseUrl}/{apiPath}";

                string userAgent = "vietbank";
                postParameters.Add("token", token);
                var response = await FormUpload.MultipartFormPost<T>(requestURL, userAgent, postParameters, "xdncode", _mcApiconfig.XDNCode, token);
                await _rpLog.InsertLog($"BeforeSendRequestUploadFile - {apiPath}", response == null ? "response = null" : response.Dump());
                return ToResponse<T>(response.Data);
            }
            catch (Exception exp)
            {
                string error = exp.InnerException == null ? exp.Dump() : exp.InnerException.Dump();
                await _rpLog.InsertLog($"BeforeSendRequestUploadFile-{apiPath}", error);
                return ToResponse<T>(null, error);
            }
        }
        protected async Task<string> GetUserToken(int userId)
        {
            _userId = userId;
            if (!string.IsNullOrWhiteSpace(_userToken))
            {
                return _userToken;
            }
            var tokenFromDb = await _rpMcredit.GetUserTokenByIdAsyncAsync(_userId);
            //tokenFromDb = null;
            if (tokenFromDb != null)
            {
                _userToken = tokenFromDb.Token;
            }
            else
            {
                var tokenFromMCApi = await Authen();
                if (tokenFromMCApi == null)
                    return null;
                _userToken = tokenFromMCApi.Obj.Token;
                await _rpMcredit.InsertUserTokenAsync(new MCreditUserToken { UserId = _userId, Token = _userToken });
            }
            return _userToken;
        }
        protected byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;
            if (!File.Exists(fileName))
                return fileData;
            using (FileStream fs = File.OpenRead(fileName))
            {
                var binaryReader = new BinaryReader(fs);
                fileData = binaryReader.ReadBytes((int)fs.Length);
            }
            return fileData;
        }

        protected void AddError(string errorMessage, params object[] traceKeys)
        {
            _process.AddError(errorMessage, traceKeys);
        }
        protected BaseResponse<T> ToResponse<T>(T data, string error = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                AddError(error);
                return BaseResponse<T>.Create(data, error);
            }
            return BaseResponse<T>.Create(data);
        }
    }
}
