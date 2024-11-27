using CommonApplicationFramework.ConfigurationHandling;

namespace CommonApplicationFramework.Common
{
    public static class APIResponse
    {
        public static ErrorResponse CreateAPIResponse(string status, string errorCode)
        {
            ErrorResponse response = new ErrorResponse { Type = status };
            if (!string.IsNullOrEmpty(errorCode))
            {
                if (!string.IsNullOrEmpty(MessageConfig.MessageSettings[errorCode]))
                {
                    response.Message = MessageConfig.MessageSettings[errorCode];
                }
                response.ErrorCode = errorCode;
                if (string.IsNullOrEmpty(response.Message))
                    response.Message = errorCode;
            }
            return response;
        }

        public static ErrorResponse CreateErrorResponse(string ErrorCode, string ErrorMessage)
        {
            return new ErrorResponse
            {
                Type = ResponseType.InvalidRequest.ToString(),
                Message = ErrorMessage,
                ErrorCode = ErrorCode
            };
        }

		public static ErrorResponse CreateErrorResponse(string ErrorCode, string ErrorMessage, string ErrorDescription, string ErrorSource)
		{
			return new ErrorResponse
			{
				Type = ResponseType.InvalidRequest.ToString(),
				Message = ErrorMessage,
				ErrorCode = ErrorCode,
				ErrorDescription = ErrorDescription,
				ErrorSource = ErrorSource
			};
		}

		public static ErrorResponse CreateAPIResponse(string validationMessage)
        {
            return new ErrorResponse
            {
                Type = ResponseType.InvalidRequest.ToString(),
                Message = validationMessage,
                ErrorCode = "INVALIDINPUT"
            };
        }

        public static ErrorResponse CreateValidationResponse(string validationMessage)
        {
            return new ErrorResponse
            {
                Type = ResponseType.InvalidRequest.ToString(),
                Message = validationMessage,
                ErrorCode = "INVALIDINPUT"
            };
        }

        public static SuccesResponse CreateAPISuccessResponse(string status, string messageCode, string responseData)
        {
            SuccesResponse response = new SuccesResponse { Type = status };
            if (!string.IsNullOrEmpty(messageCode))
            {
                if (!string.IsNullOrEmpty(MessageConfig.MessageSettings[messageCode]))
                    response.Message = MessageConfig.MessageSettings[messageCode];
            }
            if (status.Equals(ResponseType.Success.ToString()) || status.Equals(ResponseType.Created.ToString()) || status.Equals(ResponseType.Modified.ToString()) || status.Equals(ResponseType.Deleted.ToString()))
            {
                response.ResponseData = responseData;
            }
            return response;
        } 

        public static SuccesResponse CreateAPIResponse(string status, string errorCode, string responseData)
        {
            SuccesResponse response = new SuccesResponse { Type = status };
            if (!string.IsNullOrEmpty(errorCode))
            {
                if (!string.IsNullOrEmpty(MessageConfig.MessageSettings[errorCode]))
                    response.Message = MessageConfig.MessageSettings[errorCode];
            }
            if (status.Equals(ResponseType.Success.ToString()) || status.Equals(ResponseType.Created.ToString()) || status.Equals(ResponseType.Modified.ToString()) || status.Equals(ResponseType.Deleted.ToString()))
            {
                response.ResponseData = responseData;
            }
            return response;
        }     
    }
}