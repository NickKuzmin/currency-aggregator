using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.ApiModels;

namespace CurrencyAggregator.Domain.Services.Interfaces
{
    public interface ICurrencyAggregationDataProvider
    {
        Task SaveAsync(CurrencyAggregationApiModel currencyAggregationApiModel, CancellationToken cancellationToken);

        Task<List<CurrencyAggregationGroupingApiModel>> GetGroupedCurrencyAggregationsAsync(CurrencyAggregationGroupingFilter currencyAggregationGroupingFilter, CancellationToken cancellationToken);
    }
}