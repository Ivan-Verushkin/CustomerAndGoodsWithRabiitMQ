using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService
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

            services.AddMassTransit(config => {

                config.AddConsumer<ProductConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {

                    cfg.Host(Configuration["entryCredetials"]);

                    cfg.ReceiveEndpoint(Configuration["theFirstQueue"], c=> { //create an order-queue exchange

                        c.ConfigureConsumer<ProductConsumer>(ctx);
                    });
                });
            });

            services.AddMassTransitHostedService();//this is needed to configure masstransit

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // set database context
            services.AddDbContext<ProductContext>(options => options.UseSqlServer(connectionString));

            services.AddControllers();
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
