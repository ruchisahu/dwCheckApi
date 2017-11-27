﻿using System.IO;
using System.Linq;
using ClacksMiddleware.Extensions;
using dwCheckApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;


namespace dwCheckApi
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
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                    {
                        "text/plain", "application/json"
                    });
            });
            
            services.AddCustomizedMvc();
            services.AddCorsPolicy();
            services.AddDbContext();
            services.AddTransientServices();
            services.AddSwagger($"v{CommonHelpers.GetVersionNumber()}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.EnsureDatabaseIsSeeded(false);
            }
            
            app.UseResponseCompression();
            app.GnuTerryPratchett();
            app.UseCorsPolicy();
            app.UseStaticFiles();
            
            app.UseSwagger($"/swagger/v{CommonHelpers.GetVersionNumber()}/swagger.json",
                $"dwCheckApi {CommonHelpers.GetVersionNumber()}");
            
            app.UseCustomisedMvc();
        }
    }
}