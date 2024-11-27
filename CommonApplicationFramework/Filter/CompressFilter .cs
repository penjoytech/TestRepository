using CommonApplicationFramework.Logging;
using System;
using System.IO;
using System.Linq;
using System.Web.Http.Filters;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using CommonApplicationFramework.ConfigurationHandling;
using System.Net;
using CommonApplicationFramework.Common;

namespace CommonApplicationFramework.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CompressFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
			var response = context.Response.Content;
		
		   var acceptedEncoding = context.Response.RequestMessage.Headers.AcceptEncoding.First().Value;
            if (!acceptedEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase) && !acceptedEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

			string rawResponse;
			using (var stream = new StreamReader(response.ReadAsStreamAsync().Result))
			{
				stream.BaseStream.Position = 0;
				rawResponse = stream.ReadToEnd();

			}
			string rawRequest;
			using (var stream = new StreamReader(context.ActionContext.Request.Content.ReadAsStreamAsync().Result))
			{
				stream.BaseStream.Position = 0;
				rawRequest = stream.ReadToEnd();

			}
			if (context.Response.StatusCode == HttpStatusCode.BadRequest || context.Response.StatusCode == HttpStatusCode.InternalServerError)
			{
				var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(rawResponse);
				bool EnableDBErrorLogger = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableDBErrorLogger")).Value.ToString());
				if (EnableDBErrorLogger)
				{
					LogManager.Log(new LogObject()
					{
						ObjectType = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName.ToUpper(),
						Component = errorResponse.ErrorSource,
						MethodName = context.ActionContext.ActionDescriptor.ActionName.ToUpper(),
						ErrorCode = errorResponse.ErrorCode,
						ErrorMessage = errorResponse.Message,
						ErrorDescription = errorResponse.ErrorDescription,
						RequestDetails = "Query Parameters:" + context.ActionContext.Request.RequestUri.Query + " Payload:" + rawRequest,
					});
				}
				errorResponse.ErrorDescription = "";
				errorResponse.ErrorSource = "";
                //errorResponse.Type = "";
                if (errorResponse.ErrorCode.Equals("InvalidRequest"))
                {
                    errorResponse.Type = "InvalidRequest";
                    errorResponse.Message = errorResponse.Message;
                    LogManager.Log("CAF" + errorResponse.Message);
                }
                else if (errorResponse.ErrorCode.Equals("InvalidJSONInput"))
                    errorResponse.Message = errorResponse.Message;
                else if (MessageConfig.MessageSettings[errorResponse.ErrorCode] != null)
                    errorResponse.Message = MessageConfig.MessageSettings[errorResponse.ErrorCode].ToString();
                else
                    errorResponse.Message = MessageConfig.MessageSettings["ERROROCCURE"].ToString();  
				rawResponse = JsonConvert.SerializeObject(errorResponse);


			}

			context.Response.Content = new CompressedContent(new StringContent(rawResponse, Encoding.UTF8, "application/json"), System.IO.Compression.CompressionMode.Compress, acceptedEncoding);
			 
		}
    }
}