#region copyright
// <copyright file="BaseException.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion

namespace CommonApplicationFramework.ExceptionHandling
{
    #region namespace
    using System;
    #endregion

    /// <summary>
    /// This class holds error info Details.
    /// </summary>
    public sealed class ErrorInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public ErrorInfo()
        {
        }

        /// <summary>
        /// Parametrized constructor with error message, error code, error description as input
        /// </summary>
        public ErrorInfo(string errorCode, string errorMessage, string errorDescription)
        {
            _errorCode = errorCode;
            _errorMessage = errorMessage;
            _errorDescription = errorDescription;
        }

        /// <summary>
        /// Stores the Error Code for the Exception
        /// </summary>
        private string _errorCode;

        /// <summary>
        /// Gets\sets the error code
        /// </summary>
        public string ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        /// <summary>
        /// Stores the error message for the Exception
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// Gets\sets the error message
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// Stores the error description for the Exception
        /// </summary>
        private string _errorDescription;

        /// <summary>
        /// Gets\sets the error description
        /// </summary>
        public string ErrorDescription
        {
            get { return _errorDescription; }
            set { _errorDescription = value; }
        }
    }
}
