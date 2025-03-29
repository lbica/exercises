using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi.Dtos
{
    public class CustomerCreateDto
    {
        [Required]
        public string CustomerId { get; set; }

        public string Country { get; set; }

    }
}