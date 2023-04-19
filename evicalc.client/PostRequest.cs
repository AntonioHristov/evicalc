using EasyHttp.Http;
using HttpClient = EasyHttp.Http.HttpClient;
using System.Net;
using System;
using evicalc.models;

namespace evicalc.client
{
	public class PostRequest
	{
		private static void _logStrPostRequestFile(NLog.Logger logger = null)
		{
			// Because it doesn't log the name of a model file
			Common.LogStr(Environment.NewLine + "PostRequest File", logger);
		}

		// url example: "http://localhost:5217/Calculator/Add" -> url:port/Controller Name from Server/Method Post
		public static TResponse Post<TRequest, TResponse>(string url, TRequest request, string httpContentType = HttpContentTypes.ApplicationJson, NLog.Logger logger = null)
			where TResponse : class
		{
			_logStrPostRequestFile(logger);
			Common.PrintCurrentMethod(logger);

			var http = new HttpClient();
			var response = http.Post(url, request, httpContentType);
			if (response?.StatusCode != HttpStatusCode.OK)
			{
				Common.LogStr("The response is not OK", logger);
				// Something went wrong.. we should inform about it?
				if (string.IsNullOrWhiteSpace(response.RawText))
				{
					// Hey, request doesn't have the correct format, for example json '{}'..
					Common.LogStr("Request doesn't have {}" + Environment.NewLine, logger);
					Console.WriteLine(Environment.NewLine+"The request format doesn't have '{}'"+Environment.NewLine);
				}
				else
				{
					Common.LogStr($"The bad response is: {response.RawText}" + Environment.NewLine, logger);
					Console.WriteLine(Environment.NewLine+response.RawText+Environment.NewLine); // Error defined in BadRequest in the controller
				}
				return null;
			}
			Common.LogStr("The response is OK" + Environment.NewLine, logger);
			return response.StaticBody<TResponse>();
		}
	}
}