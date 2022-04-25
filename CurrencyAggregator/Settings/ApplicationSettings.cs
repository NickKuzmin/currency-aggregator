using System.ComponentModel.DataAnnotations;

namespace CurrencyAggregator.Settings
{
    public class ApplicationSettings
    {
        [Display(Name = "Currency aggregation background worker periodicity (in minutes)")]
        public int CurrencyAggregationBackgroundWorkerPeriodicity { get; set; }

        public bool CurrencyAggregationBackgroundWorkerEnabled { get; set; }
    }
}
