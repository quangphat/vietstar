using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using McreditServiceCore.Interfaces;
using VietStar.Entities.Mcredit;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using VietStar.Entities.Note;
using static VietStar.Entities.Commons.Enums;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace VietStar.Business
{
    public class MCreditBusiness : BaseBusiness, IMCreditBusiness
    {
        protected readonly IMCreditRepository _rpMCredit;
        protected readonly IMCreditService _svMcredit;
        protected readonly INoteRepository _rpNote;
        protected readonly ILogRepository _rpLog;
        protected readonly IFileProfileRepository _rpFile;
        protected readonly IServiceProvider _svProvider;
        public MCreditBusiness(IMCreditRepository mcreditRepository,
            IMCreditService mCreditService,
            INoteRepository noteRepository,
            ILogRepository logRepository,
            IServiceProvider serviceProvider,
            IFileProfileRepository fileProfileRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpMCredit = mcreditRepository;
            _svMcredit = mCreditService;
            _rpNote = noteRepository;
            _rpLog = logRepository;
            _svProvider = serviceProvider;
            _rpFile = fileProfileRepository;
        }


        public async Task<CheckSaleResponseModel> CheckSaleAsync(CheckSaleModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.SaleCode))
                return ToResponse<CheckSaleResponseModel>(null,Errors.invalid_data);
            var result = await _svMcredit.CheckSale(model.SaleCode);
            if (!result.success)
                return ToResponse<CheckSaleResponseModel>(null, result.error);
            if (model.ProfileId > 0)
            {

                if (result.success && result.data.obj != null)
                {
                    var sale = _mapper.Map<UpdateSaleModel>(result.data.obj);
                    await _rpMCredit.UpdateSaleAsyncAsync(sale, model.ProfileId);
                }
                return result.data;
            }

            return ToResponse<CheckSaleResponseModel>(null, result.data.msg.ToString());
        }

        public async Task<CheckCatResponseModel> CheckCatAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse<CheckCatResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckCat(model.Value);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<bool> IsCheckCatAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse(false, Errors.invalid_data);
            var result = await _rpMCredit.IsCheckCatAsync(model.Value);
            return result;
        }

        public async Task<CheckCICResponseModel> CheckCICAsync(CheckCICModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.IdNumber))
                return ToResponse<CheckCICResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckCIC(model.IdNumber, model.Name);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<CheckDupResponseModel> CheckDupAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse<CheckDupResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckDup(model.Value);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<CheckStatusResponseModel> CheckStatusAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse<CheckStatusResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckStatus(model.Value);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<DataPaging<List<TempProfileIndexModel>>> SearchsTemsAsync(
            DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1,
            string freeText = null, 
            string status = null, 
            int page = 1, 
            int limit = 20)
        {
            page = page <= 0 ? 1 : page;
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var profiles = await _rpMCredit.GetTempProfilesAsync(_process.User.Id, fromDate, toDate, dateType,page, limit, freeText, status);
            if (profiles == null || !profiles.Any())
            {
                return DataPaging.Create(null as List<TempProfileIndexModel>, 0);
            }
            return DataPaging.Create(profiles, profiles[0].TotalRecord);
        }

        public async Task<ProfileSearchResponse> SearchsAsync(string freeText, string status, string type, int page = 1)
        {
            var result = await _svMcredit.SearchProfiles(freeText, status, type, page);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<int> CreateDraftAsync(MCredit_TempProfileAddModel model)
        {
            if (model == null)
                return ToResponse(0, Errors.invalid_data);
            var profileExist = 0;
            if(!string.IsNullOrWhiteSpace(model.IdNumber))
            {
                profileExist = await _rpMCredit.GetProfileIdByIdNumberAsync(model.IdNumber.Trim());
            }
            else if(!string.IsNullOrWhiteSpace(model.CCCDNumber))
            {
                profileExist = await _rpMCredit.GetProfileIdByIdNumberAsync(model.CCCDNumber.Trim());
            }
            else
            {
                return ToResponse(0, Errors.missing_cmnd);
            }

            if(profileExist > 0)
            {
                return ToResponse(0, Errors.id_number_has_exist);
            }
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.CreatedBy = _process.User.Id;
            var response = await _rpMCredit.CreateDraftProfileAsync(profile);
            if (!response.success)
                return ToResponse(response);
            if (response.data > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var note = new NoteAddModel
                    {
                        CommentTime = DateTime.Now,
                        Content = model.LastNote,
                        ProfileId = response.data,
                        ProfileTypeId = (int)NoteType.MCreditTemp,
                        UserId = _process.User.Id
                    };
                    await _rpNote.AddNoteAsync(note);
                }

            }
            return response.data;
        }

        public async Task<bool> AddRefuseReasonToNoteAsync(int profileId, string type)
        {
            if (profileId <= 0 || string.IsNullOrWhiteSpace(type))
                return ToResponse(false, Errors.invalid_data);
            var response = await _svMcredit.GetProfileById(profileId.ToString());

            if (!response.success)
            {
                return ToResponse<bool>(false, response.error);
            }
            if (response.data.obj == null)
            {
                return ToResponse(false, "Không tìm thấy hồ sơ");
            }
            var profileInPortal = await _rpMCredit.GetTemProfileByMcIdAsync(response.data.obj.Id);
            if (!profileInPortal.success)
            {
                return ToResponse(false, "Không tìm thấy hồ sơ trong portal");
            }
            if (type == "refuse")
            {
                return await AddRefuseToNote(profileInPortal.data.Id, response.data.obj);
            }
            if (type == "reason")
            {
                return await AddReasonToNote(profileInPortal.data.Id, response.data.obj);
            }
            return true;

        }

        public async Task<bool> UpdateTempProfileStatusAsync(int profileId)
        {
            var temp = await _rpMCredit.GetTemProfileByIdAsync(profileId);
            if (!temp.success)
            {
                return ToResponse(false, "Không tìm thây hồ sơ trong portal");
            }
            if (string.IsNullOrWhiteSpace(temp.data.MCId))
            {
                return ToResponse(false, "Hồ sơ chưa được đẩy qua mc");
            }
            var mcProfile = await _svMcredit.GetProfileById(temp.data.MCId);
            if (!mcProfile.success)
            {
                return ToResponse(false, mcProfile.error);
            }
            int status = 0;
            try
            {
                status = Convert.ToInt32(mcProfile.data.obj.Status);
            }
            catch
            {
                status = temp.data.Status;
            }
            if (temp.data.Status != status)
            {
                var result = await _rpMCredit.UpdateTempProfileStatusAsync(temp.data.Id, status);
                return ToResponse(result);
            }
            return true;
        }

        public async Task<bool> UpdateDraftAsync(MCredit_TempProfileAddModel model)
        {
            if (model == null || model.Id <= 0)
                return ToResponse(false, Errors.invalid_data);
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.UpdatedBy = _process.User.Id;

            profile.Status = _process.User.isAdmin ? model.Status : (int)MCreditProfileStatus.Submit;
            await _rpLog.InsertLog("mcredit-UpdateDraft", model.Dump());
            var result = await _rpMCredit.UpdateDraftProfileAsync(profile);
            if (!result.success)
            {
                return ToResponse(result);
            }

            if (!string.IsNullOrWhiteSpace(model.LastNote))
            {
                var note = new NoteAddModel
                {
                    UserId = _process.User.Id,
                    ProfileId = model.Id,
                    Content = model.LastNote,
                    CommentTime = DateTime.Now,
                    ProfileTypeId = (int)NoteType.MCreditTemp
                };
                await _rpNote.AddNoteAsync(note);
            }

            return true;
        }

        public async Task<MCredit_TempProfile> GetTempProfileByIdAsync(int id)
        {
            var result = await _rpMCredit.GetTemProfileByIdAsync(id);
            return ToResponse(result);
        }




        protected async Task<bool> AddRefuseToNote(int profileId, ProfileGetByIdResponseObj profile)
        {

            if (profile != null && !string.IsNullOrWhiteSpace(profile.Refuse))
            {
                await _rpNote.AddNoteAsync(new NoteAddModel
                {
                    ProfileId = profileId,
                    CommentTime = DateTime.Now,
                    Content = profile.Refuse,
                    ProfileTypeId = (int)NoteType.MCreditTemp,
                    UserId = _process.User.Id
                });
                await _rpLog.InsertLog($"AddRefuseToNote-{profileId}", profile.Refuse);
            }
            return true;

        }
        protected async Task<bool> AddReasonToNote(int profileId, ProfileGetByIdResponseObj profile)
        {

            if (profile != null && profile.Reason != null)
            {
                var reasonName = JsonConvert.SerializeObject(profile.Reason);
                if (!string.IsNullOrWhiteSpace(reasonName))
                {
                    await _rpNote.AddNoteAsync(new NoteAddModel
                    {
                        ProfileId = profileId,
                        CommentTime = DateTime.Now,
                        Content = reasonName,
                        ProfileTypeId = (int)NoteType.MCreditTemp,
                        UserId = _process.User.Id
                    });
                }
                await _rpLog.InsertLog($"AddReasonToNote-{profileId}", reasonName);
            }


            return true;

        }

        public async Task<List<OptionSimple>> GetSimpleListByTypeAsync(string type)
        {
            var result = new List<OptionSimple>();
            switch (type)
            {
                case "city":
                    result = await _rpMCredit.GetMCCitiesSimpleListAsync();
                    break;
                case "loanperiod":
                    result = await _rpMCredit.GetMCLoanPerodSimpleListAsync();
                    break;
                case "location":
                    result = await _rpMCredit.GetMCLocationSimpleListAsync();
                    break;
                case "product":
                    result = await _rpMCredit.GetMCProductSimpleListAsync();
                    break;
                default: break;
            }
            return result;
        }

        public async Task<MCResponseModelBase> ReSendFileToECAsync(int mcProfileId)
        {
            var profile = await _rpMCredit.GetTemProfileByMcIdAsync(mcProfileId.ToString());
            if (!profile.success)
            {
                return ToResponse<MCResponseModelBase>(null, profile.error);
            }
            if(profile.data == null)
            {
                return ToResponse<MCResponseModelBase>(null, "Không tìm thấy hồ sơ portal");
            }
            if (profile == null || string.IsNullOrWhiteSpace(profile.data.MCId))
                return ToResponse<MCResponseModelBase>(null, "Hồ sơ không tồn tại hoặc chưa được gửi qua MCredit");
            var bizMedia = _svProvider.GetService<IMediaBusiness>();

            var zipFile = await bizMedia.ProcessFilesToSendToMC(profile.data.Id, Utility.FileUtils._profile_parent_folder);
            var sendFileResult = await _svMcredit.SendFiles(zipFile, profile.data.MCId);
            await _rpLog.InsertLog("ReSendFileToEC", sendFileResult != null ? sendFileResult.Dump() : "ReSendFileToEC = null");
            return ToResponse(sendFileResult);
        }

        public async Task<List<NoteObj>> GetNotesAsync(int mcProfileId)
        {
            var note = await _svMcredit.GetNotes(mcProfileId.ToString());
            if (!note.success)
            {
                return ToResponse<List<NoteObj>>(null, note.error);
            }
            return note.data.objs;
        }

        public async Task<bool> AddNoteToMcAsync(string mcId, StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value) || string.IsNullOrWhiteSpace(mcId))
                return ToResponse(false, Errors.invalid_data);
            await _svMcredit.AddNote(new NoteAddRequestModel
            {
                Content = model.Value,
                Id = mcId.Trim()
            });

            return true;
        }

        public async Task<MCResponseModelBase> SubmitToMCreditAsync(MCredit_TempProfileAddModel model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return ToResponse<MCResponseModelBase>(null, Errors.invalid_data);
                if (model.SaleId <= 0)
                    return ToResponse<MCResponseModelBase>(null, "Vui lòng chọn Sale");

                var profile = await _rpMCredit.GetTemProfileByIdAsync(model.Id);

                if (!profile.success)
                    return ToResponse<MCResponseModelBase>(null, profile.error);
                if (profile.data == null)
                    return ToResponse<MCResponseModelBase>(null, "Hồ sơ không tồn tại");

                var profileMC = _mapper.Map<MCProfilePostModel>(model);

                var files = await _rpFile.GetFilesByProfileIdAsync(model.Id, (int)ProfileType.MCredit);

                if (files == null || !files.Any())
                    return ToResponse<MCResponseModelBase>(null, "Vui lòng upload hồ sơ");

                var result = await _svMcredit.CreateProfile(profileMC);

                if (!result.success)
                    return ToResponse<MCResponseModelBase>(null, result.error);

                if(result.data == null || string.IsNullOrWhiteSpace(result.data.id))
                {
                    return ToResponse<MCResponseModelBase>(null, "Không thể gửi qua MC");
                }

                await _rpMCredit.UpdateMCIdAsync(model.Id, result.data.id.Trim(), _process.User.Id);

                await _rpFile.UpdateFileMCProfileByIdAsync(model.Id, result.data.id);

                var bizMedia = _svProvider.GetService<IMediaBusiness>();

                var zipFile = await bizMedia.ProcessFilesToSendToMC(profile.data.Id, Utility.FileUtils._profile_parent_folder);

                if (zipFile == "files_is_empty")
                {
                    return ToResponse<MCResponseModelBase>(null, "Vui lòng upload hồ sơ");
                }
                var sendFileResult = await _svMcredit.SendFiles(zipFile, result.data.id);
                if (!sendFileResult.success)
                {
                    return ToResponse<MCResponseModelBase>(null, sendFileResult.error);
                }
                return sendFileResult.data;
            }
            catch (Exception e)
            {
                var error = e.InnerException != null ? e.InnerException.Dump() : e.Dump();
                await _rpLog.InsertLog("SubmitToMCredit", error);
                return ToResponse<MCResponseModelBase>(null, error);
            }

        }

        public async Task<ProfileGetByIdResponseObj> GetMCreditProfileByIdAsync(int id)
        {
            var result = await _svMcredit.GetProfileById(id.ToString());
            if (!result.success)
            {
                return null;
            }
            //result.data.obj.IsAddrSame = "1";
            //result.data.obj.IsInsurrance = "1";
            var profile = await _rpMCredit.GetTemProfileByMcIdAsync(result.data.obj.Id);
            //if (profile == null)
            //{
            //    return ToResponse(false, "Không tìm thấy hồ sơ tương ứng trong portal");
            //}
            result.data.obj.LocalProfileId = profile.success && profile.data!=null ? profile.data.Id : 0;
            
            if (result.data.obj != null && result.data.obj.Reason != null)
            {
                result.data.obj.ReasonName = JsonConvert.SerializeObject(result.data.obj.Reason);
            }
            return result.data.obj;
        }

        public async Task<string> ExportAsync(string contentRootPath, DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , string status = null
            , string freeText = null
            , int page = 1
            , int limit = 20)
        {
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var request = new ExportRequestModel
            {
                userId = _process.User.Id,
                page = page,
                limit = limit,
                fromDate = fromDate.Value,
                toDate = toDate.Value,
                dateType = dateType,
                status = status,
                freeText = freeText
            };
            var bizCommon = _svProvider.GetService<ICommonBusiness>();
            var result = await bizCommon.ExportData<ExportRequestModel, TempProfileIndexModel>(GetDatasAsync, request, contentRootPath, "mcredit", 2);
            return result;
        }

        private async Task<List<TempProfileIndexModel>> GetDatasAsync(ExportRequestModel request)
        {
            var result = await _rpMCredit.GetTempProfilesAsync(_process.User.Id, request.fromDate, request.toDate, request.dateType, request.page, request.limit, request.freeText, request.status);
            return result;
        }
    }
}
