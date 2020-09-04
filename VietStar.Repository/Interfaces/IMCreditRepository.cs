using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.Mcredit;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IMCreditRepository
    {
        Task<BaseResponse<bool>> UpdateTempProfileStatusAsync(int profileId, int status);
        Task<bool> IsCheckCatAsync(string productCode);
        Task<int> GetProfileIdByIdNumberAsync(string idNumber);
        Task<bool> InsertPeopleWhoCanViewProfileAsync(int profileId, string peopleIds);
        Task<BaseResponse<MCredit_TempProfile>> GetTemProfileByMcIdAsync(string id);
        Task<List<OptionSimple>> GetMCProfileStatusListAsync();
        Task<bool> UpdateSaleAsyncAsync(UpdateSaleModel model, int profileId);
        Task<int> CountTempProfilesAsync(string freeText, int userId, string status = null);
        Task<List<TempProfileIndexModel>> GetTempProfilesAsync(int userId, 
            DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1,
            int page =1, 
            int limit =10, 
            string freeText = null, 
            string status = null);
        Task<BaseResponse<int>> CreateDraftProfileAsync(MCredit_TempProfile model);
        Task<BaseResponse<bool>> UpdateDraftProfileAsync(MCredit_TempProfile model);
        Task<BaseResponse<bool>> UpdateMCIdAsync(int id, string mcId, int updatedBy);
        Task<bool> DeleteMCTableDatasAsync(int type);
        Task<MCreditUserToken> GetUserTokenByIdAsyncAsync(int userId);
        Task<bool> InsertUserTokenAsync(MCreditUserToken model);
        Task<bool> InsertLocationsAsync(List<MCreditlocations> locations);
        Task<bool> InsertProductsAsync(List<MCreditProduct> products);
        Task<bool> InsertLoanPeriodsAsync(List<MCreditLoanPeriod> loanPeriods);
        Task<bool> InsertProfileStatusAsync(List<OptionSimple> status);
        Task<bool> InsertCitiesAsync(List<MCreditCity> cities);
        Task<List<OptionSimple>> GetMCProductSimpleListAsync();
        Task<List<OptionSimple>> GetMCLocationSimpleListAsync();
        Task<List<OptionSimple>> GetMCLoanPerodSimpleListAsync();
        Task<List<OptionSimple>> GetMCCitiesSimpleListAsync();
        Task<BaseResponse<MCredit_TempProfile>> GetTemProfileByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int profileId);
    }
}

