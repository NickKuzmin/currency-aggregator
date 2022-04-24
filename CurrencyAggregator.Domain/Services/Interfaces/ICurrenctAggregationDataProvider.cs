using System.Threading;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.ApiModels;

namespace CurrencyAggregator.Domain.Services.Interfaces
{
    public interface ICurrencyAggregationDataProvider
    {
        Task SaveAsync(CurrencyAggregationApiModel currencyAggregationApiModel, CancellationToken cancellationToken);
    }
}