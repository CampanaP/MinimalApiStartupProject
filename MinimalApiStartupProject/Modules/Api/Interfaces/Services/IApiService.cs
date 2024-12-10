using MinimalApiStartupProject.Modules.Api.Enums;
using MinimalApiStartupProject.Modules.Api.Models;

namespace MinimalApiStartupProject.Modules.Api.Interfaces.Services
{
    public interface IApiService
    {
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
        Task<TReturn?> DeleteAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class;

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
        Task<T?> GetAsync<T>(string endpointUrl, string? endpointParameters = null, List<RequestHeader>? headers = null, ResponseType? responseType = null, CancellationToken cancellationToken = default);

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
        Task<TReturn?> PatchAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class;

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
        Task<TReturn?> PostAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class;

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
        Task<TReturn?> PutAsync<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, ResponseType? responseType = null, List<RequestHeader>? headers = null, CancellationToken cancellationToken = default) where TParameter : class;
    }
}