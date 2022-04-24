using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.Constants;
using CurrencyAggregator.Domain.Services.Interfaces;

namespace CurrencyAggregator.Domain.Services.Implementation
{
    public class CurrencyAggregationHttpClient : ICurrencyAggregationHttpClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public CurrencyAggregationHttpClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<double?> GetCurrencyPairValueAsync(string currencyPairType)
        {
            var currConvCurrencyPairTypes = currencyPairType == CurrencyPairTypes.USDRUB
                ? CurrConvCurrencyPairTypes.USDRUB
                : CurrConvCurrencyPairTypes.EURRUB;

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://free.currconv.com/api/v7/convert?q={currConvCurrencyPairTypes}&compact=ultra&apiKey=SECRET")
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
