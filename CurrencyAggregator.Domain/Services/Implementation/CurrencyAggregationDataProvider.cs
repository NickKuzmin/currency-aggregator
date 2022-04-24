using System;
using System.Threading;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.ApiModels;
using CurrencyAggregator.Domain.DataContext;
using CurrencyAggregator.Domain.Entities;
using CurrencyAggregator.Domain.Services.Interfaces;

namespace CurrencyAggregator.Domain.Services.Implementation
{
    public class CurrencyAggregationDataProvider : ICurrencyAggregationDataProvider
    {
        private readonly ApplicationContext _applicationContext;

        public CurrencyAggregationDataProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task SaveAsync(CurrencyAggregationApiModel currencyAggregationApiModel, CancellationToken cancellationToken)
        {
            using var context = _applicationContext;

            var currencyAggregation = new CurrencyAggregation
            {
                TypeCode = currencyAggregationApiModel.TypeCode,
                Value = currencyAggregationApiModel.Value,
                Date = currencyAggregationApiModel.Date
            };

            context.CurrencyAggregations.Add(currencyAggregation);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
