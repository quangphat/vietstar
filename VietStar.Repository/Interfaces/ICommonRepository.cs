using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface ICommonRepository
    {
        Task<List<ExportFramework>> GetExportFrameworkByTypeAsync(string profileType, int orgId);
        //Task<SystemConfig> GetSystemConfigByCodeAsync(string code);
        Task<List<ImportExcelFrameWorkModel>> GetImportFrameworkByTypeAsync(int type);
        Task<List<OptionSimple>> GetListForCheckCustomerDuplicateAsync();
        Task<List<OptionSimple>> GetProfileStatusByRoleId(string profileType, int orgId, int roleId = 0);
        Task<List<OptionSimple>> GetProfileStatusByRoleCode(string profileType, int orgId, string roleCode = null);
    }
}

