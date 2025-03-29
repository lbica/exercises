using webapi.Dtos;
using webapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Runtime.InteropServices;


namespace webapi.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //var param = context.ActionArguments.SingleOrDefault(p => p.Value is IEntity);
            //if (param.Value == null)
            //{
            //    context.Result = new BadRequestObjectResult("Object is null");
            //    return;
            //}

            //if (!context.ModelState.IsValid)
            //{
            //    var modelState = context.ModelState;

            //    var errors = getModelStateErrors(modelState);

            //    context.Result = new BadRequestObjectResult(new ResponseDto<object>()
            //    {
            //        Title = "Error",
            //        Data = null,
            //        ErrorDetail = new ErrorDetail()
            //        {
            //            Title = "Internal Server Error.",
            //            StatusCode = (int)HttpStatusCode.BadRequest,
            //            Details = "Something went wrong.",
            //            Errors = new Dictionary<string, object>
            //            {
            //                { "error", errors}
            //            }
            //        }

            //    });
            //}
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        private IDictionary<string, string[]> getModelStateErrors(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errors = modelState
                        .Where(e => e.Value?.Errors.Count > 0) // Filter model state entries that have errors
                        .ToDictionary(
                            kvp => kvp.Key, // Use the property name as the key
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() // Use the error messages as the value
                        );
                return errors;
            }

            return null;

        }
    }
}
