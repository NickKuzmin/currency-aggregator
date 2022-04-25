using System.Collections.Generic;
using CurrencyAggregator.Domain.ApiModels;
using MediatR;

namespace CurrencyAggregator.Commands
{
    public class GetCurrencyAggregationCommand : IRequest<List<CurrencyAggregationGroupingApiModel>>
    {
        public CurrencyAggregationGroupingFilter CurrencyAggregationGroupingFilter { get; set; }
    }
}
