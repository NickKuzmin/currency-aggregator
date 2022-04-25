using System;
using System.Threading;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.ApiModels;
using CurrencyAggregator.Domain.Constants;
using CurrencyAggregator.Domain.Services.Interfaces;
using CurrencyAggregator.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CurrencyAggregator.Services
{
    public class CurrencyAggregationBackgroundWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ApplicationSettings _applicationSettings;
        private readonly string[] _currencyPairs = { CurrencyPairTypes.EURRUB, CurrencyPairTypes.USDRUB };

        public CurrencyAggregationBackgroundWorker(IServiceScopeFactory serviceScopeFactory, ApplicationSettings applicationSettings)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_applicationSettings.CurrencyAggregationBackgroundWorkerEnabled)
                {
                    foreach (var currencyPair in _currencyPairs)
                    {
                        double? currencyPairValue;
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var currencyAggregationHttpClient = scope.ServiceProvider.GetService<ICurrencyAggregationHttpClient>();
                            currencyPairValue = await currencyAggregationHttpClient.GetCurrencyPairValueAsync(currencyPair);
                        }

                        var now = DateTime.Now;

                        var currencyAggregationApiModel = new CurrencyAggregationApiModel
                        {
                            TypeCode = currencyPair,
                            Value = currencyPairValue,
                            Date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0)
                        };

                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var currencyAggregationDataProvider = scope.ServiceProvider.GetService<ICurrencyAggregationDataProvider>();
                            await currencyAggregationDataProvider.SaveAsync(currencyAggregationApiModel, stoppingToken);
                        }
                    }
                }

                await Task.Delay(1000 * 60 * _applicationSettings.CurrencyAggregationBackgroundWorkerPeriodicity, stoppingToken);
            }
        }
    }
}
