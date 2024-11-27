#region copyright
// <copyright file="BaseException.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion
using System;
using System.Runtime.Serialization;

namespace CommonApplicationFramework.ExceptionHandling
{
    /// <summary>
    /// Base exception class to be inherited by Exception class
    /// </summary>
    [Serializable]
    public class BaseException : Exception
    {
        /// <summary>
        /// Stores the Error Code for the Exception
        /// </summary>
        private string _errorCode;
        public string ErrorCode
        {
            get
            {
                return _errorCode;
            }
            set
            {
                _errorCode = value;
            }
        }
        /// <summary>
        /// Stores the Response Type for the Exception
        /// </summary>
        private string _responseType;
        public string ResponseType
        {
            get
            {
                return _responseType;
            }
            set
            {
                _responseType = value;
            }
        }

        /// <summary>
        /// Stores the error message for the exception
        /// </summary>
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

        /// <summary>
        /// Stores the error description for the exception
        /// </summary>
        private string _errorDescription;
        public string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
            set
            {
                _errorDescription = value;
            }
        }        

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public BaseException()
        {
        }

        /// <summary>
        /// Overloaded constructor with error message as input 
        /// </summary>
        /// <param name="message">string</param>
        public BaseException(string message)
            : base(message)
        {
          _errorMessage = message;
        }
             
        /// <summary>
        /// Constructor with error message, errorcode and description as input
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="errorCode">string</param>
        /// <param name="description">string</param>
        public BaseException(string errorCode, string message, string description)
        {
            _errorCode = errorCode;
            _errorMessage = message;
            _errorDescription = description;
        }

        /// <summary>
        /// Constructor with error message and exception as input
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="ex">Exception</param>        
        public BaseException(string message, Exception ex)
        {
            _errorMessage = message;
            _errorDescription = ex.StackTrace;
        }

        /// <summary>
        /// Constructor with error message and exception as input
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="ex">Exception</param>        
        public BaseException(string responseType, string errCode)
        {
            _responseType = responseType;
            _errorCode = errCode;
        }
    }
}
