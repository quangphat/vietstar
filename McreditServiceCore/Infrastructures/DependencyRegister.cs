using McreditServiceCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Infrastructures
{
    public static class DependencyRegister
    {
        public static void RegisterMCService(this IServiceCollection services)
        {
            services.AddScoped<IMCreditService, MCreditLoanService>();
        }
    }
}
