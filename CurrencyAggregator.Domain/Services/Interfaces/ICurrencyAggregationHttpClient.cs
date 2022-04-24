using System.Threading.Tasks;

namespace CurrencyAggregator.Domain.Services.Interfaces
{
    public interface ICurrencyAggregationHttpClient
    {
        Task<double?> GetCurrencyPairValueAsync(string currencyPairType);
    }
}
