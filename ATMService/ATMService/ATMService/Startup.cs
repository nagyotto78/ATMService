using ATMService.DAL;
using ATMService.DAL.Repositories;
using ATMService.Services;
using ATMService.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace ATMService
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ATMService", Version = "v1" });
            });

            // Database context and repositories
            string connectionString = DataUtils.ResolveDbConnectionString(Configuration, Environment.CurrentDirectory, ATMDbContext.CONNECTION_STRING_KEY);
            services.AddDbContextPool<ATMDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IMoneyDenominationRepository, MoneyDenominationRepository>();
            services.AddTransient<IMoneyStorageRepository, MoneyStorageRepository>();

            // Add ATM Business logic service
            services.AddTransient<IATMService, Services.ATMService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ATMService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
