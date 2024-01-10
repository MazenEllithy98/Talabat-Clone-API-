namespace Talabat.API.Errors
{
    public class ApiErrorResponse
    {
        
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }

        public ApiErrorResponse(int statusCode , string? errorMessage = null )
        {
            StatusCode = statusCode ;
            ErrorMessage = errorMessage?? GetDefaultMessageForStatusCode(statusCode) ;
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "you have made a Bad Request",
                401 => "you are not Authorized",
                404 => "Resource Not Found",
                500 => "the server encountered an unexpected condition that prevented it from fulfilling the request.",
                _ => null
            };

        }
    }
}
