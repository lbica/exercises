using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Models;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace webapi.ActionFilters
{

    public class HttpResponseException : Exception
    {
        public HttpResponseException(int statusCode, object value = null) =>
            (StatusCode, Value) = (statusCode, value);

        public int StatusCode { get; }

        public object Value { get; }
    }
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = getModelStateErrors(context.ModelState);
                context.Result = new ObjectResult(new ResponseDto<object>()
                {
                    Title = "Error",
                    Data = null,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorDetail = new ErrorDetail()
                    {
                        Title = "Bad request.",
                        Details = "Error validation.",
                        Errors = errors
                    }
                });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.ExceptionHandled = true;
            }
        }

        private string[] getModelStateErrors(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errors = modelState
                        .Where(e => e.Value?.Errors.Count > 0) // Filter model state entries that have errors
                        .Select(e => String.Join("\n", e.Value.Errors.Select(p => p.ErrorMessage).ToArray()))
                        .ToArray();
                        //.ToDictionary(
                        //    kvp => kvp.Key, // Use the property name as the key
                        //    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() // Use the error messages as the value
                        //);
                return errors;
            }

            return null;

        }

    }

}
