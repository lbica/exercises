using webapi.Dtos;
using webapi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace webapi.Middlewares
{
    public class GlobalExceptionErrorMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<GlobalExceptionErrorMiddleware> _logger;

        public GlobalExceptionErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (context.Response.StatusCode != (int)HttpStatusCode.OK)
                {
                    //logger.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsJsonAsync(new ResponseDto<object>()
                    {
                        Title = "Error",
                        Data = null,
                        StatusCode = context.Response.StatusCode,
                        ErrorDetail = new ErrorDetail()
                        {
                            Title = "Internal Server Error.",
                            Details = exception.Message,
                            Errors = new string[] { exception.Message } 
                        }

                    });
                }
            }
        }
    }
}
