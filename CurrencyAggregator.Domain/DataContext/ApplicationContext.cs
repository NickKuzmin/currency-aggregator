using CurrencyAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyAggregator.Domain.DataContext
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        public DbSet<CurrencyAggregation> CurrencyAggregations { get; set; }
    }
}