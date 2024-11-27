#region copyright
// <copyright file="InValidArgumentException.cs" company="Pentechs.com">
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
    /// This class handles the invalid argument exception and inherit from BaseException.
    /// </summary>
    public class InValidArgumentException : BaseException
    {
        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message as input
        /// </summary>
        public InValidArgumentException(string message) : base(message) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, exception as input
        /// </summary>
        public InValidArgumentException(string message, Exception ex) : base(message, ex) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, error code, description as input
        /// </summary>
        public InValidArgumentException(string errorCode, string message, string description) : base(errorCode, message, description) { }
    }
}
