using System;

namespace CurrencyAggregator.Domain.ApiModels
{
    public class CurrencyAggregationApiModel
    {
        public int Id { get; set; }

        public string TypeCode { get; set; }

        public double? Value { get; set; }
        
        public DateTime Date { get; set; }
    }
}
