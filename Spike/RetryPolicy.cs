using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Optional;
using Polly;
using Storefront.Services.PatternsAndPractices.CQRS;

namespace Storefront.Services.Services
{
    public interface IHttpStatusCodeRetryPolicy<TResult>
        where TResult : IServiceResult
    {
        Task<Option<TResult, HttpStatusCode>> ExecuteAsync(
            Func<CancellationToken, Task<Option<TResult, HttpStatusCode>>> action,
            CancellationToken cancellationToken);
    }

    public class HttpStatusCodeRetryPolicy<TResult> : IHttpStatusCodeRetryPolicy<TResult> where TResult : IServiceResult
    {
        private readonly IAsyncPolicy<Option<TResult, HttpStatusCode>> _policyAsync;

        public HttpStatusCodeRetryPolicy()
        {
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout, // 504
                HttpStatusCode.TooManyRequests //429
            };

            // We don't need fallback or circuit breaker in this case because there is no danger of a downstream cascade failure and we don't need to save failed requests to try again later.  
            _policyAsync =   
                Policy.HandleResult<Option<TResult, HttpStatusCode>>(x =>
                    x.Match(result => false, exception => httpStatusCodesWorthRetrying.Contains(exception)))
                .WaitAndRetryAsync(5, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public async Task<Option<TResult, HttpStatusCode>> ExecuteAsync(Func<CancellationToken, Task<Option<TResult, HttpStatusCode>>> action,
            CancellationToken cancellationToken)
        {
            return await _policyAsync.ExecuteAsync(async ct => await action(cancellationToken), cancellationToken);
        }
    }
}
