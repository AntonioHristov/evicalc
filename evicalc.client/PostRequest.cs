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
		{
			var http = new HttpClient();

			var response = http.Post(url, request, httpContentType);
			if (response?.StatusCode != HttpStatusCode.OK)
			{
				// Something went wrong.. we should inform about it?
				if (string.IsNullOrWhiteSpace(response.RawText))
				{
					// Oye.. que nos han mandado algo que ni si quiera tiene '{}'..
					Console.WriteLine("The request format doesn't have '{}'");
				}
				else
				{
					Console.WriteLine(response.RawText); // Error defined in BadRequest in the controller
				}
				//return nothing or return the text in the if above... I don't know how to do it
			}
			return response.StaticBody<TResponse>();
		}
	}
}