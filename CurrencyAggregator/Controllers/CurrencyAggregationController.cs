using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrencyAggregator.Commands;
using CurrencyAggregator.Domain.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyAggregationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyAggregationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<List<CurrencyAggregationGroupingApiModel>> Get([FromQuery] CurrencyAggregationGroupingFilter filter, CancellationToken cancellationToken)
        {
            var getCurrencyAggregationCommand = new GetCurrencyAggregationCommand()
            {
                CurrencyAggregationGroupingFilter = filter
            };
            var result = await _mediator.Send(getCurrencyAggregationCommand, cancellationToken);

            return result;
        }
    }
}
