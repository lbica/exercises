namespace webapi.Exceptions
{
    public class WebApiServiceException: ApplicationException
    {
        public WebApiServiceException() : base() { }
        public WebApiServiceException(string message) : base(message) { }

        public WebApiServiceException(string message, Exception innerException): base(message, innerException) { }


 

    }
}
