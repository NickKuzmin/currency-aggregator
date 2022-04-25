using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.ApiModels;
using CurrencyAggregator.Domain.DataContext;
using CurrencyAggregator.Domain.Entities;
using CurrencyAggregator.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<CurrencyAggregationGroupingApiModel>> GetGroupedCurrencyAggregationsAsync(CurrencyAggregationGroupingFilter currencyAggregationGroupingFilter, CancellationToken cancellationToken)
        {
            using var context = _applicationContext;

            var now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0)
                .AddMinutes(-(int) currencyAggregationGroupingFilter.CurrencyAggregationGroupingPeriodicity);

            var endDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            var currencyAggregations = await context.CurrencyAggregations
                .Where(x => startDate <= x.Date && x.Date <= endDate)
                .ToListAsync(cancellationToken);

            /***
             * Entity Framework 7 (now renamed to Entity Framework Core 1.0)
             * does not yet support GroupBy() for translation to GROUP BY in generated SQL.
             * Any grouping logic will run on the client side, which could cause a lot of data to be loaded.
             ***/

            var result = (from currencyAggregation in currencyAggregations
                    group currencyAggregation by currencyAggregation.TypeCode
                    into g
                    select new CurrencyAggregationGroupingApiModel
                    {
                        TypeCode = g.Key,
                        StartDateTimeInterval = g.Select(x => x.Date).Min(),
                        EndDateTimeInterval = g.Select(x => x.Date).Max(),
                        FirstValue = g.OrderBy(x => x.Date).Select(x => x.Value).First(),
                        LastValue = g.OrderBy(x => x.Date).Select(x => x.Value).Last(),
                        MinValue = g.Select(x => x.Value).Min(),
                        MaxValue = g.Select(x => x.Value).Max()
                    }).ToList();

            return result;
        }
    }
}
