#region copyright
// <copyright file="RepositoryException.cs" company="Pentechs.com">
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
    /// The custom class to capture all Repository exceptions
    /// </summary>
    public class RepositoryException : BaseException
    {
        /// <summary>
        /// parameterless constructor
        /// </summary>
        public RepositoryException(){ }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message as input
        /// </summary>
        public RepositoryException(string message) : base(message) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, error code, description as input
        /// </summary>
        public RepositoryException(string errorCode, string message, string description) : base(errorCode, message, description) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, exception as input
        /// </summary>
        public RepositoryException(string message, Exception ex) : base(message, ex) { }
    }
}