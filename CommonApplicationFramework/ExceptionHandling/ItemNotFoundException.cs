#region copyright
// <copyright file="ItemNotFoundException.cs" company="Pentechs.com">
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
    /// This class handles the item not found exception and inherit from BaseException.
    /// </summary>
    public class ItemNotFoundException : BaseException
    {
        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message as input
        /// </summary>
        public ItemNotFoundException(string message) : base(message) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, exception as input
        /// </summary>
        public ItemNotFoundException(string message, Exception ex) : base(message, ex) { }

        /// <summary>
        /// This parametrized constructor inherits overloaded constructor with error message, error code, description as input
        /// </summary>
        public ItemNotFoundException(string errorCode, string message, string description) : base(errorCode, message, description) { }
    }
}
