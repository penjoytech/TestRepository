#region copyright
// <copyright file="BusinessException.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion
namespace CommonApplicationFramework.ExceptionHandling
{
    #region namespaces
    using System;
    #endregion

    /// <summary>
    /// BusinessException class to be inherited by BaseException class
    /// </summary>
    public class BusinessException : BaseException
    {
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public BusinessException() { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message as input
        /// </summary>
        public BusinessException(string message) : base(message) { }


        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message as input
        /// </summary>
        public BusinessException(string  responseType, string errCode) : base(responseType, errCode) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, error code, description as input
        /// </summary>
        public BusinessException(string errorCode, string message, string description) : base(errorCode, message, description) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, exception as input
        /// </summary>
        public BusinessException(string message, Exception ex) : base(message, ex) { }
    }
}
