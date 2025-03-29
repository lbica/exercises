using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi.Dtos
{
    public class CustomerUpdateDto
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string Country { get; set; }

    }
}