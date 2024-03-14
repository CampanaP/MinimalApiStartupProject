using Serilog;
using System.Text.Json;
using System.Text;
using System.Xml.Serialization;
using $safeprojectname$.Modules.Api.Interfaces.Services;
using $safeprojectname$.Modules.Api.Models;

namespace $safeprojectname$.Modules.Api.Services
{
	public class ApiService : IApiService
	{
		public ApiService()
		{
		}

		private async Task<TReturn?> SendRequest<TReturn, TParameter>(string endpointUrl, HttpMethod httpMethod, TParameter? requestContent = null, List<RequestHeader>? headers = null, bool isResponseXml = false) where TParameter : class
		{
			TReturn? data = default;

			try
			{
				using (HttpClient client = new HttpClient())
				{
					if (httpMethod == HttpMethod.Get)
					{
						endpointUrl = endpointUrl + requestContent;
					}

					HttpRequestMessage request = new HttpRequestMessage(httpMethod, endpointUrl);

					if (requestContent != null && httpMethod != HttpMethod.Get)
					{
						request.Content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
					}

					if (headers?.Any() ?? false)
					{
						foreach (RequestHeader header in headers)
						{
							request.Headers.Add(header.Name, header.Value);
						}
					}

					HttpResponseMessage response = await client.SendAsync(request);
					if (!response.IsSuccessStatusCode)
					{
						Log.Error($"{nameof(ApiService)} - {httpMethod.Method}: {endpointUrl} - {JsonSerializer.Serialize(requestContent)} - {response.StatusCode}");

						return data;
					}

					string responseContent = await response.Content.ReadAsStringAsync();
					if (string.IsNullOrWhiteSpace(responseContent))
					{
						return data;
					}

					if (isResponseXml)
					{
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(TReturn));
						data = (TReturn?)xmlSerializer.Deserialize(new StringReader(responseContent));
					}
					else
					{
						data = JsonSerializer.Deserialize<TReturn>(responseContent);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error($"{nameof(ApiService)} - {httpMethod.Method}: {endpointUrl} - {JsonSerializer.Serialize(requestContent)} - {ex.Message}");

				throw;
			}

			return data;
		}

		public async Task<T?> Get<T>(string endpointUrl, string? endpointParameters = null, List<RequestHeader>? headers = null, bool isResponseXml = false)
		{
			return await SendRequest<T, string>(endpointUrl, HttpMethod.Get, endpointParameters, headers, isResponseXml);
		}

		public async Task<TReturn?> Post<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class
		{
			return await SendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Post, requestContent, headers);
		}

		public async Task<TReturn?> Put<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class
		{
			return await SendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Put, requestContent, headers);
		}

		public async Task<TReturn?> Patch<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class
		{
			return await SendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Patch, requestContent, headers);
		}

		public async Task<TReturn?> Delete<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class
		{
			return await SendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Delete, requestContent, headers);
		}
	}
}