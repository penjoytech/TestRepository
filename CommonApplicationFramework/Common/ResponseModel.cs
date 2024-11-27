namespace CommonApplicationFramework.Common
{
    public class Response
    {
        public string Type { get; set; }

        public string Message { get; set; } 
    }

    public class SuccesResponse : Response
    {  
        public string ResponseData { get; set; }
    }

    public class ErrorResponse : Response
    {       
        public string ErrorCode { get; set; }

		public string ErrorDescription { get; set; }

		public string ErrorSource { get; set; }
	}
}
