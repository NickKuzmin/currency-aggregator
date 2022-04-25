using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.Constants;
using CurrencyAggregator.Domain.Services.Interfaces;
using CurrencyAggregator.Domain.Settings;

namespace CurrencyAggregator.Domain.Services.Implementation
{
    public class CurrencyAggregationHttpClient : ICurrencyAggregationHttpClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly CurrConvHttpSettings _currConvHttpSettings;

        public CurrencyAggregationHttpClient(IHttpClientFactory clientFactory, CurrConvHttpSettings currConvHttpSettings)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _currConvHttpSettings = currConvHttpSettings ?? throw new ArgumentNullException(nameof(currConvHttpSettings));
        }

        public async Task<double?> GetCurrencyPairValueAsync(string currencyPairType)
        {
            var currConvCurrencyPairTypes = currencyPairType == CurrencyPairTypes.USDRUB
                ? CurrConvCurrencyPairTypes.USDRUB
                : CurrConvCurrencyPairTypes.EURRUB;

            var url = _currConvHttpSettings.Url
                .Replace("{currConvCurrencyPairTypes}", currConvCurrencyPairTypes);

            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
                {
                    { "Accept", "application/json" }
                }
            };

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<Dictionary<string, double>>(responseStream);

                if (result != null && result.TryGetValue(currConvCurrencyPairTypes, out var currencyPairValue))
                {
                    return currencyPairValue;
                }
            }

            return null;
        }
    }
}
