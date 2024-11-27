#region copyright
// <copyright file="ValidationException.cs" company="Pentechs.com">
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
    /// The custom class to capture all Validation exceptions
    /// </summary>    
    public class ValidationException : BaseException
    {

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public ValidationException()
        {
        }  

        /// <summary>
        /// Overloaded constructor with error message as input
        /// </summary>
        /// <param name="message">string</param>
        public ValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Overloaded constructor with error code, message and description as input
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="errorCode">string</param>
        /// <param name="description">string</param>
        public ValidationException(
           string errorCode, string message, string description)
            : base(errorCode,message,description)
        {
        }   
    }
}