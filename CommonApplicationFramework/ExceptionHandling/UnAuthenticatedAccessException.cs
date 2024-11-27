#region copyright
// <copyright file="UnAuthenticatedAccessException.cs" company="Pentechs.com">
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
    /// Custom class to capture all the exception while Authentication 
    /// </summary>    
    public class UnAuthenticatedAccessException : BaseException
    {
        /// <summary>
        /// parameterless constructor
        /// </summary>
        public UnAuthenticatedAccessException(){ }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message as input
        /// </summary>
        public UnAuthenticatedAccessException(string message) : base(message) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, error code, description as input
        /// </summary>
        public UnAuthenticatedAccessException(string errorCode, string message, string description) : base(errorCode, message, description) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, exception as input
        /// </summary>
        public UnAuthenticatedAccessException(string message, Exception ex) : base(message, ex) { }
    }
}
