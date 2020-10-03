using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VietStar.Client.Models;
using VietStar.Repository.Interfaces;

namespace VietStar.Client.Infrastructures
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureexceptionHandler(this IApplicationBuilder app, ILogRepository rpLog)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await rpLog.InsertLog("exception-error", contextFeature.Error.ToString());
                        await context.Response.WriteAsync(new ExceptionDetail()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error.",
                            Error = contextFeature.Error.ToString()
                        }.ToString());
                    }
                });
            });
        }
    }
}
