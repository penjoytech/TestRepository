#region copyright
// <copyright file="UnhandledExceptionLogger.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion
namespace CommonApplicationFramework.ExceptionHandling
{
    #region namespace
    using System;
    using System.Web.Http.ExceptionHandling;
    using CommonApplicationFramework.Logging;
    #endregion

    /// <summary>
    /// Defines a global logger for unhandled exceptions.
    /// </summary>
    
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        /// Writes log record to the database synchronously.
        public override void Log(ExceptionLoggerContext context)
        {
            try
            {
                var request = context.Request;
                var exception = context.Exception;
                LogManager.Log(context.RequestContext == null ?
                   null : context.RequestContext.Principal.Identity.Name);
                LogManager.Log(exception);
            }
            catch
            {
                // logger shouldn't throw an exception!!!
            }
        }
    }
}
