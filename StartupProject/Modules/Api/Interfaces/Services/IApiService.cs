using $safeprojectname$.Modules.Api.Models;

namespace $safeprojectname$.Modules.Api.Interfaces.Services
{
	public interface IApiService
	{
		Task<T?> Get<T>(string endpointUrl, string? endpointParameters = null, List<RequestHeader>? headers = null, bool isResponseXml = false);

		Task<TReturn?> Post<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class;

		Task<TReturn?> Put<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class;

		Task<TReturn?> Patch<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class;

		Task<TReturn?> Delete<TReturn, TParameter>(string endpointUrl, TParameter? requestContent = null, List<RequestHeader>? headers = null) where TParameter : class;
	}
}