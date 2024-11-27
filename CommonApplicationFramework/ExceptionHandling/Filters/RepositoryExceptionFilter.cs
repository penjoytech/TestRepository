namespace CommonApplicationFramework.ExceptionHandling.Filters
{
    #region Namespace
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using CommonApplicationFramework.ExceptionHandling;
    using CommonApplicationFramework.Logging;
    #endregion

    public class RepositoryAPIExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            LogManager.Log(context.Exception.Message);
            LogManager.Log(context.Exception.StackTrace);
            if (context.Exception is ItemNotFoundException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = context.Exception.Message
                };
                throw new HttpResponseException(resp);
            }
            if (context.Exception is InValidArgumentException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "ArgumentNullException"
                };
                throw new HttpResponseException(resp);
            }
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "UnhandledException"
                };
                throw new HttpResponseException(resp);
            }
        }
    }
}