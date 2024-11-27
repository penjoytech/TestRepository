#region Namespace
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using CommonApplicationFramework.ExceptionHandling;
using CommonApplicationFramework.Logging;
#endregion

namespace CommonApplicationFramework.ExceptionHandling.Filters
{
	public class CommonExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext context)
		{
			string rawRequest = string.Empty;
			using (var stream = new StreamReader(context.Request.Content.ReadAsStreamAsync().Result))
			{
				stream.BaseStream.Position = 0;
				rawRequest = stream.ReadToEnd();
			}
			Exception innerEx = context.Exception;
			Exception exception = null;
			if (innerEx != null)
			{
				while (innerEx.InnerException != null)
				{
					exception = innerEx.InnerException;
					innerEx = exception;
				}
			}
			LogManager.Log(new LogObject()
			{				
				ObjectType = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,
				Component = "CONTROLLER",
				MethodName = context.ActionContext.ActionDescriptor.ActionName,
				ErrorCode = "",
				ErrorDescription = context.Exception.StackTrace,
				ErrorMessage = innerEx.Message,
				RequestDetails = context.Request.RequestUri.Query + " " + rawRequest,
			});
			if (context.Exception is BusinessException)
			{
				var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
				{
					Content = new StringContent(context.Exception.Message),
					ReasonPhrase = context.Exception.Message
				};
				throw new HttpResponseException(resp);
			}
			//if (context.Exception is RepositoryException)
			//{
			//	var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
			//	{
			//		Content = new StringContent(context.Exception.Message),
			//		ReasonPhrase = context.Exception.Message
			//	};
			//	throw new HttpResponseException(resp);
			//}
		}
	}
}
