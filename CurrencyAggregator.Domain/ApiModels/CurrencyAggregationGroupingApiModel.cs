using System;

namespace CurrencyAggregator.Domain.ApiModels
{
    public class CurrencyAggregationGroupingApiModel
    {
        public string TypeCode { get; set; }

        public DateTime StartDateTimeInterval { get; set; }

        public DateTime EndDateTimeInterval { get; set; }

        public double? MinValue { get; set; }

        public double? MaxValue { get; set; }

        public double? FirstValue { get; set; }

        public double? LastValue { get; set; }
    }
}
