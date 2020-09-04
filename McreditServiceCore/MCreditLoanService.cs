using HttpService;
using McreditServiceCore.Interfaces;
using VietStar.Entities.Mcredit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Mcredit;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using Microsoft.Extensions.Options;

namespace McreditServiceCore
{
    public class MCreditLoanService : MCreditServiceBase, IMCreditService
    {
        public MCreditLoanService(IMCreditRepository mCreditRepository,
            ILogRepository logRepository,
            IOptions<McreditApi> option,
            CurrentProcess currentProcess) : base(mCreditRepository, logRepository, option, currentProcess)
        {

        }

        public async Task<BaseResponse<CheckCatResponseModel>> CheckCat(string taxNumber)
        {
            var model = new CheckCatRequestModel { taxNumber = taxNumber };
            var result = await BeforeSendRequest<CheckCatResponseModel, CheckCatRequestModel>(_mcApiconfig.CheckCATApi, model);
            return result;
        }
        public async Task<BaseResponse<CheckSaleResponseModel>> CheckSale(string salecode)
        {
            var model = new CheckSaleRequestModel { idCode = salecode };
            var result = await BeforeSendRequest<CheckSaleResponseModel, CheckSaleRequestModel>(_mcApiconfig.CheckSaleAPI, model);
            return result;
        }
        public async Task<BaseResponse<CheckDupResponseModel>> CheckDup(string value)
        {
            var model = new CheckDupRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckDupResponseModel, CheckDupRequestModel>(_mcApiconfig.CheckDupApi, model);
            return result;
        }
        public async Task<BaseResponse<CheckCICResponseModel>> CheckCIC(string idNumber, string name)
        {
            var model = new CheckCICRequestModel { IdNumber = idNumber, name = name };
            var result = await BeforeSendRequest<CheckCICResponseModel, CheckCICRequestModel>(_mcApiconfig.CheckCICApi, model);
            return result;
        }
        public async Task<BaseResponse<CheckStatusResponseModel>> CheckStatus(string value)
        {
            var model = new CheckStatusRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckStatusResponseModel, CheckStatusRequestModel>(_mcApiconfig.CheckStatusApi, model);
            return result;
        }
        public async Task<BaseResponse<ProfileSearchResponse>> SearchProfiles(string freetext, string status, string type, int page)
        {
            var model = new ProfileSearchRequestModel
            {
                str = freetext,
                page = page,
                status = status,
                type = type
            };
            var result = await BeforeSendRequest<ProfileSearchResponse, ProfileSearchRequestModel>(_mcApiconfig.SearchProfileApi, model);
            return result;
        }
        public async Task<BaseResponse<ProfileGetByIdResponse>> GetProfileById(string profileId)
        {
            var model = new ProfileGetByIdRequest
            {
                Id = profileId
            };
            var result = await BeforeSendRequest<ProfileGetByIdResponse, ProfileGetByIdRequest>(_mcApiconfig.GetProfileByIdApi, model);
            return result;
        }
        public async Task<BaseResponse<ProfileAddResponse>> CreateProfile(MCProfilePostModel obj)
        {
            var model = new ProfileAddRequest
            {
                Obj = obj
            };
            var result = await BeforeSendRequest<ProfileAddResponse, ProfileAddRequest>(_mcApiconfig.CreateProfileApi, model);
            return result;
        }
        public async Task<BaseResponse<GetFileUploadResponse>> GetFileUpload(GetFileUploadRequest model)
        {
            var result = await BeforeSendRequest<GetFileUploadResponse, GetFileUploadRequest>(_mcApiconfig.GetFileToUploadApi, model);
            return result;
        }
        public async Task<BaseResponse<NoteResponseModel>> GetNotes(string profileId)
        {
            var model = new NoteRequestModel
            {
                Id = profileId
            };
            await _rpLog.InsertLog("mcredit-GetNote-request", model.Dump());

            var result = await BeforeSendRequest<NoteResponseModel, NoteRequestModel>(_mcApiconfig.GetNotesApi, model);
            await _rpLog.InsertLog("mcredit-GetNote-result", result.Dump());
            return result;
        }
        public async Task<BaseResponse<NoteAddResponseModel>> AddNote(NoteAddRequestModel model)
        {
            var result = await BeforeSendRequest<NoteAddResponseModel, NoteAddRequestModel>(_mcApiconfig.AddNoteApi, model);
            await _rpLog.InsertLog("mcredit-AddNote", result.Dump());
            return result;
        }
        public async Task<BaseResponse<MCResponseModelBase>> SendFiles(string fileName, string profileId)
        {
            // You need to do this download if your file is on any other server otherwise you can convert that file directly to bytes  
            //WebClient wc = new WebClient();
            //byte[] bytes = wc.DownloadData(fileName); 

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("file", new FileParameter(data, "18.zip", "application/zip"));
            postParameters.Add("id", profileId);
            return await BeforeSendRequestUploadFile<MCResponseModelBase, MCreditRequestModelBase>(_mcApiconfig.UploadFileApi, postParameters);
        }
    }
}
