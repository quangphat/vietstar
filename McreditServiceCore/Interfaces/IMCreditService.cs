using VietStar.Entities.Mcredit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Mcredit;

namespace McreditServiceCore.Interfaces
{
    public interface IMCreditService
    {
        Task<BaseResponse<ProfileGetByIdResponse>> GetProfileById(string profileId);
        Task<BaseResponse<CheckSaleResponseModel>> CheckSale(string salecode);
        Task<BaseResponse<CheckCatResponseModel>> CheckCat(string taxNumber);
        Task<BaseResponse<CheckDupResponseModel>> CheckDup(string value);
        Task<BaseResponse<CheckCICResponseModel>> CheckCIC(string idNumber, string name);
        Task<BaseResponse<CheckStatusResponseModel>> CheckStatus(string value);
        Task<BaseResponse<ProfileSearchResponse>> SearchProfiles(string freetext, string status, string type, int page);
        Task<BaseResponse<ProfileAddResponse>> CreateProfile(MCProfilePostModel obj);
        Task<BaseResponse<string>> AuthenByUserId(int userId,int[] tableToUpdateIds = null);
        Task<BaseResponse<GetFileUploadResponse>> GetFileUpload(GetFileUploadRequest model);
        Task<BaseResponse<MCResponseModelBase>> SendFiles( string fileName, string profileId);
        Task<BaseResponse<NoteResponseModel>> GetNotes(string profileId);
        Task<BaseResponse<NoteAddResponseModel>> AddNote(NoteAddRequestModel model);
    }
}
