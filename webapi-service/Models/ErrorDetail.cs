using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi.Models
{
    public class ErrorDetail
    {

        public string Title { get; set; }

        //public int StatusCode { get; set; }

        public string Details { get; set; }

        public string[] Errors { get; set; }


        //public List<string> ErrorCodes { get; set; }
        //public List<string> ErrorMessages { get; set; }


        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);

            //var errorsStr = string.Empty;

            //foreach (var error in Errors)
            //{
            //    errorsStr += string.Format("{0}, {1}", error.Key, error.Value);
            //}

            //string result = string.Format("Title: {0}, StatusCode: {1}, Details: {2}, Errors: {3}", Title,
            //StatusCode, Details, errorsStr);


            //return result;
        }


    }
}
