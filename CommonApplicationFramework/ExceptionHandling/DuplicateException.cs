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
    public class DuplicateException : BaseException
    {

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public DuplicateException()
        {
        }  

        /// <summary>
        /// Overloaded constructor with error message as input
        /// </summary>
        /// <param name="message">string</param>
        public DuplicateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Overloaded constructor with error code, message and description as input
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="errorCode">string</param>
        /// <param name="description">string</param>
        public DuplicateException(
            string message, string errorCode, string description)
            : base(message, errorCode, description)
        {
        }   
    }
}