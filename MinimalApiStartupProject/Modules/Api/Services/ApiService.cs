using MinimalApiStartupProject.Infrastructures.Attributes;
using MinimalApiStartupProject.Infrastructures.StringExtensions;
using MinimalApiStartupProject.Modules.Api.Enums;
using MinimalApiStartupProject.Modules.Api.Interfaces.Services;
using MinimalApiStartupProject.Modules.Api.Models;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace MinimalApiStartupProject.Modules.Api.Services
{
    [ScopedLifetime]
    internal class ApiService : IApiService
    {
        //https://timdeschryver.dev/blog/refactor-your-net-http-clients-to-typed-http-clients
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Async method to send request to external api
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="endpointUrl"></param>
        /// <param name="httpMethod"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseType"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<TReturn?> SendRequestAsync<TReturn, TParameter>(string endpointUrl, HttpMethod httpMethod, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            TReturn? data = default;

            if (httpMethod == HttpMethod.Get)
            {
                endpointUrl = endpointUrl + requestContent;
            }

            try
            {
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

                    using (HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            _logger.LogException(nameof(ApiService), httpMethod.Method, JsonSerializer.Serialize(requestContent), $"{endpointUrl} Response status code: {response.StatusCode}");

                            return data;
                        }

                        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                        if (string.IsNullOrWhiteSpace(responseContent))
                        {
                            return data;
                        }

                        if (responseType.HasValue)
                        {
                            switch (responseType)
                            {
                                case ResponseType.Xml:
                                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(TReturn));
                                    data = (TReturn?)xmlSerializer.Deserialize(new StringReader(responseContent));
                                    break;
                                case ResponseType.Json:
                                    data = JsonSerializer.Deserialize<TReturn>(responseContent);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(nameof(ApiService), httpMethod.Method, JsonSerializer.Serialize(requestContent), $"{endpointUrl} Exception message: {ex.Message}");

                throw;
            }

            return data;
        }

        /// <summary>
        /// Async method to send delete request to external api
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="endpointUrl"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseType"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TReturn?> DeleteAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await SendRequestAsync<TReturn, TParameter>(endpointUrl, HttpMethod.Delete, requestContent, responseType, headers, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Async method to send get request to external api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpointUrl"></param>
        /// <param name="endpointParameters"></param>
        /// <param name="headers"></param>
        /// <param name="responseType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T?> GetAsync<T>(string endpointUrl, string? endpointParameters = null, List<RequestHeader>? headers = null, ResponseType? responseType = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<T, string>(endpointUrl, HttpMethod.Get, endpointParameters, responseType, headers, cancellationToken);
        }

        /// <summary>
        /// Async method to send patch request to external api
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="endpointUrl"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseType"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TReturn?> PatchAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await SendRequestAsync<TReturn, TParameter>(endpointUrl, HttpMethod.Patch, requestContent, responseType, headers, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Async method to send post request to external api
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="endpointUrl"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseType"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TReturn?> PostAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await SendRequestAsync<TReturn, TParameter>(endpointUrl, HttpMethod.Post, requestContent, responseType, headers, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Async method to send put request to external api
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="endpointUrl"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseType"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TReturn?> PutAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class
        {
            return await SendRequestAsync<TReturn, TParameter>(endpointUrl, HttpMethod.Put, requestContent, responseType, headers, cancellationToken: cancellationToken);
        }
    }
}