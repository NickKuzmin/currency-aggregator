using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyAggregator.Domain.Entities
{
    [Table("currencyaggregation", Schema = "dbo")]
    public class CurrencyAggregation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("typecode")]
        public string TypeCode { get; set; }

        [Column("value")]
        public double? Value { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }
}
