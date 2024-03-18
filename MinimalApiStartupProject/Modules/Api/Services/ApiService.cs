using $safeprojectname$.Infrastructures.ServiceExtensions.Attributes;
using $safeprojectname$.Modules.Api.Interfaces.Services;
using $safeprojectname$.Modules.Api.Models;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace $safeprojectname$.Modules.Api.Services
{
    [ScopedLifetime]
    internal class ApiService : IApiService
    {
        public ApiService()
        {
        }

        private async Task<TReturn?> sendRequest<TReturn, TParameter>(string endpointUrl, HttpMethod httpMethod, TParameter? requestContent = null, List<RequestHeader>? headers = null, bool isResponseXml = false, CancellationToken cancellationToken = default) where TParameter : class
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

                    using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, endpointUrl))
                    {
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

                        using (HttpResponseMessage response = await client.SendAsync(request, cancellationToken))
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                Log.Error("{apiService} - {httpMethod}: {endpointUrl} - {requestContentJson} - {responseStatusCode}", new object[] { nameof(ApiService), httpMethod.Method, endpointUrl, JsonSerializer.Serialize(requestContent), response.StatusCode }, null);

                                return data;
                            }

                            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
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
                }
            }
            catch (Exception ex)
            {
                Log.Error("{apiService} - {httpMethod}: {endpointUrl} - {requestContentJson} - {responseStatusCode}", new object[] { nameof(ApiService), httpMethod.Method, endpointUrl, JsonSerializer.Serialize(requestContent), ex.Message }, ex);

                throw;
            }

            return data;
        }

        public async Task<TReturn?> Delete<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await sendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Delete, requestContent, headers, cancellationToken: cancellationToken);
        }

        public async Task<T?> Get<T>(string endpointUrl, string? endpointParameters = null, List<RequestHeader>? headers = null, bool isResponseXml = false, CancellationToken cancellationToken = default)
        {
            return await sendRequest<T, string>(endpointUrl, HttpMethod.Get, endpointParameters, headers, isResponseXml, cancellationToken);
        }

        public async Task<TReturn?> Patch<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await sendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Patch, requestContent, headers, cancellationToken: cancellationToken);
        }

        public async Task<TReturn?> Post<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await sendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Post, requestContent, headers, cancellationToken: cancellationToken);
        }

        public async Task<TReturn?> Put<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await sendRequest<TReturn, TParameter>(endpointUrl, HttpMethod.Put, requestContent, headers, cancellationToken: cancellationToken);
        }
    }
}