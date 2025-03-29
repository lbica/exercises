using webapi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace webapi.Dtos
{
    public class ResponseDto<T>
    {
        [JsonPropertyOrder(1)]
        //[JsonPropertyName("Title")]
        public String Title { get; set; }
        [JsonPropertyOrder(2)]
        //[JsonPropertyName("Data")]
        public T Data { get; set; }

        [JsonPropertyOrder(3)]
        public int StatusCode { get; set; }

        [JsonPropertyOrder(4)]
        //[JsonPropertyName("ErrorDetail")]
        public ErrorDetail ErrorDetail { get; set; }

        public override string ToString()
        {
            //create an options for CamelCase
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            return System.Text.Json.JsonSerializer.Serialize(this, options);
        }

    }
}
