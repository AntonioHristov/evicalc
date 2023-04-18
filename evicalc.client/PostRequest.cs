using EasyHttp.Http;
using HttpClient = EasyHttp.Http.HttpClient;
using System.Net;
using System;

namespace evicalc.client
{
	public class PostRequest
	{
		// url example: "http://localhost:5217/Calculator/Add" -> url:port/Controller Name from Server/Method Post
		public static TResponse Post<TRequest, TResponse>(string url, TRequest request, string httpContentType = HttpContentTypes.ApplicationJson)
			where TResponse : class
		{
			var http = new HttpClient();
			var response = http.Post(url, request, httpContentType);
			if (response?.StatusCode != HttpStatusCode.OK)
			{
				// Something went wrong.. we should inform about it?
				if (string.IsNullOrWhiteSpace(response.RawText))
				{
					// Hey, request doesn't have the correct format, for example json '{}'..
					Console.WriteLine(Environment.NewLine+"The request format doesn't have '{}'"+Environment.NewLine);
				}
				else
				{
					Console.WriteLine(Environment.NewLine+response.RawText+Environment.NewLine); // Error defined in BadRequest in the controller
				}
				return null;
			}
			return response.StaticBody<TResponse>();
		}
	}
}