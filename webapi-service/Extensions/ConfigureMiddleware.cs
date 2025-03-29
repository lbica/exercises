using webapi.Dtos;
using webapi.Middlewares;
using webapi.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace webapi.Extensions
{
    public static class ConfigureMiddleware
    {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            // other middleware registrations
            app.UseMiddleware<GlobalExceptionErrorMiddleware>();
        }

        //    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        //    {
        //        app.UseExceptionHandler(appError =>
        //        {
        //            appError.Run(async context =>
        //            {
        //                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                context.Response.ContentType = "application/json";
        //                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //                if (contextFeature != null)
        //                {
        //                    //logger.LogError($"Something went wrong: {contextFeature.Error}");
        //                    await context.Response.WriteAsync(new ResponseDto<object>()
        //                    {
        //                        Title = "Error",
        //                        Data = null,
        //                        ErrorDetail = new ErrorDetail()
        //                        {
        //                            Title = "Internal Server Error",
        //                            StatusCode = context.Response.StatusCode,
        //                            Details = contextFeature.Error.Message,
        //                            Errors = new Dictionary<string, object>
        //                            {
        //                                { "errors", contextFeature.Error.Message}
        //                            }
        //                        }

        //                    }.ToString());
        //                }
        //            });
        //        });
        //    }
    }
}
