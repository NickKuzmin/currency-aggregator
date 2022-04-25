using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrencyAggregator.Domain.ApiModels;
using CurrencyAggregator.Domain.Services.Interfaces;
using MediatR;

namespace CurrencyAggregator.Commands
{
    public class GetCurrencyAggregationCommandHandler : IRequestHandler<GetCurrencyAggregationCommand, List<CurrencyAggregationGroupingApiModel>>
    {
        private readonly ICurrencyAggregationDataProvider _currencyAggregationDataProvider;

        public GetCurrencyAggregationCommandHandler(ICurrencyAggregationDataProvider currencyAggregationDataProvider)
        {
            _currencyAggregationDataProvider = currencyAggregationDataProvider ?? throw new ArgumentNullException(nameof(currencyAggregationDataProvider));
        }

        public async Task<List<CurrencyAggregationGroupingApiModel>> Handle(GetCurrencyAggregationCommand request, CancellationToken cancellationToken)
        {
            return await _currencyAggregationDataProvider.GetGroupedCurrencyAggregationsAsync(request.CurrencyAggregationGroupingFilter, cancellationToken);
        }
    }
}
