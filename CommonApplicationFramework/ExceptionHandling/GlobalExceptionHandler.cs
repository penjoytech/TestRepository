#region copyright
// <copyright file="GlobalExceptionHandler.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion

namespace CommonApplicationFramework.ExceptionHandling
{
	using CommonApplicationFramework.Logging;
	#region namespaces
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    #endregion

    /// <summary>
    /// This class handles the global exception and inherit from ExceptionHandler.
    /// </summary>
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is ArgumentNullException)
            {
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "ArgumentNullException"
                };
                context.Result = new ErrorMessageResult(context.Request, result);
            }
            else
            {
				LogManager.Log(context.Exception.Message);
				var result = new HttpResponseMessage(HttpStatusCode.InternalServerError)
			   {
				  
                   Content = new StringContent("UnexpectedError"),
                   ReasonPhrase = "UnexpectedError"
               };
                context.Result = new ErrorMessageResult(context.Request, result);
            }
        }
    }

    /// <summary>
    /// This class handles the error message and inherit from IHttpActionResult.
    /// </summary>
    public class ErrorMessageResult : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private HttpResponseMessage _httpResponseMessage;

        public ErrorMessageResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _request = request;
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }
}
