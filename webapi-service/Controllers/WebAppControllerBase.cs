using AutoMapper;
using webapi.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    //[Route("api/v1/")]
    //[ApiController]
    public class WebAppControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> logger;

        protected readonly static string CONST_TITLE_SUCCESS_RESPONSE = "Sucessfully return data.";

        protected readonly IMapper mapper;

        public WebAppControllerBase(ILogger<T> logger, IMapper map)
        {
            this.logger = logger;
            mapper = map;
        }

        //protected IDictionary<string, string[]> getModelStateErrors()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState
        //                .Where(e => e.Value?.Errors.Count > 0) // Filter model state entries that have errors
        //                .ToDictionary(
        //                    kvp => kvp.Key, // Use the property name as the key
        //                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() // Use the error messages as the value
        //                );
        //        return errors;
        //    }

        //    return null;

        //}
    }
}
