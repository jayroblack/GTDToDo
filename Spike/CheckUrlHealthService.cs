using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optional;
using Storefront.Services.Abstractions.Config;
using Storefront.Services.Abstractions.CQRS.Provider.Model;
using Storefront.Services.PatternsAndPractices.CQRS;

namespace Storefront.Services.Services.StatusService
{
    public class CheckUrlHealthServiceArg : IServiceArgs<CheckUrlHealthServiceResult>
    {
        public OnBoardingDetailsView View { get; }

        public CheckUrlHealthServiceArg(OnBoardingDetailsView view)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
        }
    }

    public class CheckUrlHealthServiceResult : IServiceResult
    {
        public bool IsSuccessful { get; }

        public CheckUrlHealthServiceResult(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }

    public class CheckUrlHealthService : IServiceAsyncWrappable<CheckUrlHealthServiceArg, CheckUrlHealthServiceResult, HttpStatusCode>
    {
        private readonly ILogger<CheckUrlHealthService> _logger;
        private readonly IStatusPageConfiguration _configuration;
        private readonly IQueryHandlerAsync<GetBearerTokenForCheckUrlHealthQueryArg, GetBearerTokenForCheckUrlHealthQueryResult> _getBearerTokenQueryHandlerAsync;

        public CheckUrlHealthService(ILogger<CheckUrlHealthService> logger,
            IStatusPageConfiguration configuration,
            IQueryHandlerAsync<GetBearerTokenForCheckUrlHealthQueryArg, GetBearerTokenForCheckUrlHealthQueryResult> getBearerTokenQueryHandlerAsync)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _getBearerTokenQueryHandlerAsync = getBearerTokenQueryHandlerAsync ?? throw new ArgumentNullException(nameof(getBearerTokenQueryHandlerAsync));
        }

        public async Task<Option<CheckUrlHealthServiceResult, HttpStatusCode>> Run(CheckUrlHealthServiceArg arg)
        {
            var statusPageDetails =
                _configuration.StatusPage.FirstOrDefault(x => x.Key.Equals(arg.View.ProductName)).Value;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var headers = statusPageDetails.Headers;
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                (await _getBearerTokenQueryHandlerAsync.Run(new GetBearerTokenForCheckUrlHealthQueryArg(arg.View)))
                    .MatchSome(some => client.DefaultRequestHeaders.Add("Authorization", some.Token));
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(statusPageDetails.URL);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                if (response != null && response.IsSuccessStatusCode)
                    return Option.Some<CheckUrlHealthServiceResult, HttpStatusCode>(new CheckUrlHealthServiceResult(true));
                return Option.None<CheckUrlHealthServiceResult, HttpStatusCode>(HttpStatusCode.InternalServerError);
            }
        }
    }
}