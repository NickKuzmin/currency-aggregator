using CurrencyAggregator.Domain.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CurrencyAggregator.Domain.Services.Implementation;
using CurrencyAggregator.Domain.Services.Interfaces;
using CurrencyAggregator.Domain.Settings;
using CurrencyAggregator.Services;
using CurrencyAggregator.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CurrencyAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ApplicationContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("ApplicationContext"));
                });
            services.AddControllers();
            services.AddMediatR(typeof(Startup));
            services.AddHttpClient();
            services.AddScoped<ICurrencyAggregationHttpClient, CurrencyAggregationHttpClient>();
            services.AddScoped<ICurrencyAggregationDataProvider, CurrencyAggregationDataProvider>();
            services.AddHostedService<CurrencyAggregationBackgroundWorker>();

            AddConfigurationSettings(services);
        }

        public void AddConfigurationSettings(IServiceCollection services)
        {
            var currConvHttpSettings = Configuration.GetSection("CurrConvHttpSettings")
                .Get<CurrConvHttpSettings>();
            var applicationSettings = Configuration.GetSection("ApplicationSettings")
                .Get<ApplicationSettings>();

            services.AddSingleton(provider => currConvHttpSettings);
            services.AddSingleton(provider => applicationSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
