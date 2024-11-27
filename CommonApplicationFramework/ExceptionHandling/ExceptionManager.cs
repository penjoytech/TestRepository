#region copyright
// <copyright file="ExceptionManager.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion
namespace CommonApplicationFramework.ExceptionHandling
{
    #region namespaces
    using System;
    using CommonApplicationFramework.Logging;
    using CommonApplicationFramework.Common;
    using CommonApplicationFramework.ConfigurationHandling;
    using CommonApplicationFramework.Notification;
    #endregion

    /// <summary>
    /// This class handles the raised exception and logs it.
    /// </summary>
    public static class ExceptionManager
    {
        /// <summary>This method logs the input exception with an exception policy</summary>
        /// <param name="exception">Exception</param>
        /// <param name="policyName">string</param>
        public static void HandleException(Exception exception, string Code = "MFL")
        {
            try
            {
                // Pass the inner exception if it is present
                Exception innerEx = exception;
                if (innerEx != null)
                {
                    while (innerEx.InnerException != null)
                    {
                        exception = innerEx.InnerException;
                        innerEx = exception;
                    }
                    LogManager.Log(innerEx);
                    bool EnableErrorNotification = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableErrorNotification")).Value.ToString());
                    if (EnableErrorNotification)
                    {
                        string Notificationuser = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("NotificationUsers")).Value.ToString();

                        EmailSender.SendNotificationUser(Notificationuser, innerEx.Message + "\n" + innerEx.StackTrace, Code);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        /// <summary>This method logs the input exception with an exception policy</summary>
        /// <param name="exception">Exception</param>
        /// <param name="policyName">string</param>
        public static ErrorResponse HandleException(Exception exception, string errorCode, string Code = "MFL")
        {
            Exception innerEx = exception;
            if (innerEx != null)
            {
                while (innerEx.InnerException != null)
                {
                    exception = innerEx.InnerException;
                    innerEx = exception;
                }
                LogManager.Log(innerEx, Code);
                bool EnableErrorNotification = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableErrorNotification")).Value.ToString());
                if (EnableErrorNotification)
                {
                    string Notificationuser = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("NotificationUsers")).Value.ToString();
                    EmailSender.SendNotificationUser(Notificationuser, innerEx.Message + "\n" + innerEx.StackTrace, Code);
                }
            }

			ErrorResponse response = new ErrorResponse();
			 
			if (!string.IsNullOrEmpty(errorCode))
			{
				if ( MessageConfig.MessageSettings[errorCode] != null)
					response.Message =  MessageConfig.MessageSettings[errorCode].ToString();
				else
					response.Message = MessageConfig.MessageSettings["ERROROCCURE"].ToString();
				response.ErrorCode = errorCode;
			}
			response.ErrorDescription = innerEx.Message + innerEx.StackTrace;
			response.ErrorSource = "CONTROLLER";

			//if (CommonApplicationFramework.ConfigurationHandling.MessageConfig.MessageSettings[errorCode] != null)
			//             return CommonApplicationFramework.ConfigurationHandling.MessageConfig.MessageSettings[errorCode].ToString();
			//         else
			//             return CommonApplicationFramework.ConfigurationHandling.MessageConfig.MessageSettings["ERROROCCURE"].ToString();
			return response;
		}

        /// <summary>This method logs the input exception with an exception policy</summary>
        /// <param name="exception">Exception</param>
        /// <param name="policyName">string</param>
        public static Response HandleException(Exception exception, string status, string errorCode, string Code = "MFL")
        {
            Exception innerEx = exception;
            if (innerEx != null)
            {
                while (innerEx.InnerException != null)
                {
                    exception = innerEx.InnerException;
                    innerEx = exception;
                }
                LogManager.Log(innerEx, Code);
                bool EnableErrorNotification = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableErrorNotification")).Value.ToString());
                if (EnableErrorNotification == true)
                {
                    string Notificationuser = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("NotificationUsers")).Value.ToString();
                    EmailSender.SendNotificationUser(Notificationuser, innerEx.Message + "\n" + innerEx.StackTrace, Code);
                }
            }
			//         ErrorResponse response = new ErrorResponse();
			//         response.Type = status;
			//         if (!string.IsNullOrEmpty(errorCode))
			//         {
			//             response.Message = MessageConfig.MessageSettings[errorCode];
			//             response.ErrorCode = errorCode;
			//         }
			//response.ErrorDescription = innerEx.Message + innerEx.StackTrace;
			//response.ErrorSource = "CONTROLLER";

			ErrorResponse response = new ErrorResponse();

			if (!string.IsNullOrEmpty(errorCode))
			{
				if (MessageConfig.MessageSettings[errorCode] != null)
					response.Message = MessageConfig.MessageSettings[errorCode].ToString();
				else
					response.Message = MessageConfig.MessageSettings["ERROROCCURE"].ToString();
				response.ErrorCode = errorCode;
			}
			response.ErrorDescription = innerEx.Message + innerEx.StackTrace;
			response.ErrorSource = "CONTROLLER";

			return response;
        }

        /// <summary>Constructor which raises custom exception by excepting error code and Custom Exception instance as parameter.</summary>
        /// <param name="errorCode">string-is used to read error message and description from config file</param>
        /// <param name="baseException">Custom Exception to be raised</param>
        public static void RaiseException(string errorCode, BaseException baseException)
        {
            ErrorInfo errorInfo = GetErrorInfo(errorCode);
            baseException.ErrorCode = errorCode;
            baseException.ErrorDescription = errorInfo.ErrorDescription;
            baseException.ErrorMessage = errorInfo.ErrorMessage;
            throw baseException;
        }

        /// <summary>Constructor which raises custom exception by excepting error code,Custom Exception and Original exception instance as parameter.</summary>
        /// <param name="errorCode">string-is used to read error message from config file</param>
        /// <param name="baseException">Custom Exception to be raised</param>
        /// <param name="ex">Exception - is used to get the error descrption</param>
        public static void RaiseException(string errorCode, BaseException baseException, Exception ex)
        {
            ErrorInfo errorInfo = GetErrorInfo(errorCode);
            baseException.ErrorCode = errorCode;
            baseException.ErrorDescription = ex.StackTrace;
            baseException.ErrorMessage = ex.Message;
            throw baseException;
        }

        public static ErrorInfo RaiseException(BaseException baseException, string errorCode, string errorMessage, string errorDescription)
        {
            baseException.ErrorCode = errorCode;
            baseException.ErrorDescription = errorDescription;
            baseException.ErrorMessage = errorMessage;
            throw baseException;
        }

        /// <summary>Exception base class definition implemention to return error info object</summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public static ErrorInfo GetErrorInfo(string errorCode)
        {
            ErrorInfo errorInfo = new ErrorInfo();
            errorInfo.ErrorCode = errorCode;
            errorInfo.ErrorDescription = "Retrieve error description from xml";
            errorInfo.ErrorMessage = "Retrieve error message description from xml";
            return errorInfo;
        }
    }
}
