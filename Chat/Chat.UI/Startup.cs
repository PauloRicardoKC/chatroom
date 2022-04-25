using Chat.Domain.Interfaces;
using Chat.Infra.Data.DataBases;
using Chat.Infra.Data.DataBases.Context;
using Chat.UI.Configurations;
using Chat.UI.Configurations.Ioc;
using Chat.UI.Configurations.MessageBus;
using Chat.UI.Consumers;
using GreenPipes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Chat.UI
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddIoC(Configuration);

            services.AddBroker(
                configure =>
                {
                    configure.SetBrokerOptions(broker =>
                    {
                        broker.AddHostedService = true;
                        broker.HostOptions = new BrokerHostOptions
                        {
                            Host = Configuration.GetConnectionString("RabbitConnection")
                        };

                        broker.PrefetchCount = 1;
                    });
                },
                consumers =>
                {
                    consumers.Add<StockQuoteConsumer>(r => r.Interval(3, TimeSpan.FromSeconds(10)));
                },
                queues =>
                {
                    queues.Map<StockQuote>();
                }
            );

            services.AddHostedService<Worker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopmentOrDocker())
            {
                app.UseDeveloperExceptionPage();
                DbInitializer.SeedData(IocConfiguration.GetDbContext(Configuration, "DefaultConnection"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.UseChat();
            });
        }
    }
}
