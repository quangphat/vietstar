using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.FileProfile;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business.Interfaces
{
    public interface ICommonBusiness
    {
        Task<string> ExportData<TRequest, TData>(Func<TRequest, Task<List<TData>>> funcGetData,
            TRequest request,
            string folder,
            string profileType,
            int rowIndex)
            where TData : Pagination
            where TRequest : ExportRequestModelBase;
        Task<List<DynamicParameters>> ReadXlsxFileAsync(MemoryStream stream, ProfileType profileType, int ignoreRow, int minRowRequire);
        Task<List<OptionSimple>> GetPartnerscheckDupAsync();
        Task<List<OptionSimple>> GetPartnersAsync();
        Task<List<OptionSimple>> GetProductsAsync(int partnerId);
        Task<List<OptionSimple>> GetSalesAsync();
        Task<List<OptionSimple>> GetCouriersAsync();
        Task<List<OptionSimple>> GetProvincesAsync();
        Task<List<OptionSimple>> GetDistrictsAsync(int provinceId);
        Task<List<OptionSimple>> GetStatusList(string profileType);
        
    }
}
