using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Repository.Interfaces;

namespace VietStar.Repository.Infrastructures
{
    public static class DependencyRegister
    {
        public static void RegisterRepository(this IServiceCollection services)
        {
            services.AddSingleton<ICheckDupRepository, CheckDupRepository>();
            services.AddSingleton<IPartnerRepository, PartnerRepository>();
            services.AddSingleton<IRevokeDebtRepository, RevokeDebtRepository>();
            services.AddSingleton<IMCreditRepository, MCreditRepository>();
            services.AddSingleton<ICourierRepository, CourierRepository>();
            services.AddSingleton<ICompanyRepository, CompanyRepository>();
            services.AddSingleton<INoteRepository, NoteRepository>();
            services.AddSingleton<IProfileNotificationRepository, ProfileNotificationRepository>();
            services.AddSingleton<ICommonRepository, CommonRepository>();
            services.AddSingleton<IPartnerRepository, PartnerRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ILocationRepository, LocationRepository>();
            services.AddSingleton<IFileProfileRepository, FileProfileRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IGroupRepository, GroupRepository>();
            services.AddSingleton<ILogRepository, LogRepository>();
            services.AddSingleton<IProfileRepository, ProfileRepository>();
        }
    }
}
