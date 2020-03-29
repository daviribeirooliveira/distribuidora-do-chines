using DistribuidoraDoChines.Commons.Services;
using DistribuidoraDoChines.Mvc.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistribuidoraDoChines.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbConnection")));

            services.AddDbContext<DistribuidoraDoChinesContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DistribuidoraDoChinesConnection")));

            services
                .AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("ApplicationDbConnection"), "SELECT 1", "MSSql")
                .AddMySql(Configuration.GetConnectionString("DistribuidoraDoChinesConnection"), "MySql");

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseDeveloperExceptionPage();
            
            app.UseHealthCheckService();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}