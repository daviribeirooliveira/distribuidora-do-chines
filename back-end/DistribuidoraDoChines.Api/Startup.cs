using System.IO.Compression;
using DistribuidoraDoChines.Api.Data.Context;
using DistribuidoraDoChines.Api.Helpers;
using DistribuidoraDoChines.Commons.Models;
using DistribuidoraDoChines.Commons.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Api
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
            services
                .AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            
            services
                .AddDbContext<DistribuidoraDoChinesContext>(options => options
                    .UseMySql(Configuration.GetConnectionString("DistribuidoraDoChinesConnection")));

            services
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression();

            services
                .AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services
                .AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration")
                    .Get<EmailConfiguration>())
                .AddSingleton<IEmailService, EmailService>();
            
            services
                .AddHealthChecks()
                .AddMySql(Configuration.GetConnectionString("DistribuidoraDoChinesConnection"), "MySql");
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distribuidora Do Chinês", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            app.UseAuthentication();
            
            app.UseResponseCompression();
            
            app.UseHealthCheckService();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            
            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers();
            });
        }
    }
}