using webapi.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi.Dtos
{
    public class ErrorDetailsDto
    {
        public ErrorDetailsDto(ErrorDetail errorDetail)
        {
            ErrorDetail = errorDetail;
        }

        public ErrorDetail ErrorDetail { get; set; }


    }
}
