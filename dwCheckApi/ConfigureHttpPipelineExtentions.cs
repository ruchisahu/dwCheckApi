﻿using dwCheckApi.Common;
using dwCheckApi.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace dwCheckApi
{
    /// <summary>
    /// This class is based on some of the suggestions bty K. Scott Allen in
    /// his NDC 2017 talk https://www.youtube.com/watch?v=6Fi5dRVxOvc
    /// </summary>
    public static class ConfigureHttpPipelineExtentions
    {
        private static string CorsPolicyName => new CorsConfiguration().GetCorsPolicyName();
        
        public static void UseCustomisedMvc(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void UseCorsPolicy(this IApplicationBuilder applicationBuilder, string corsPolicyName = null)
        {
            applicationBuilder.UseCors(corsPolicyName ?? CorsPolicyName);
        }

        public static int EnsureDatabaseIsSeeded(this IApplicationBuilder applicationBuilder,
            bool autoMigrateDatabase)
        {
            // seed the database using an extension method
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DwContext>();
                if (autoMigrateDatabase)
                {
                    //context.Database.Migrate();
                }
                return context.EnsureSeedData();
            }
        }

        /// <summary>
        /// Used to tell the <see cref="IApplicationBuilder"/> to use Swagger and the Swagger UI
        /// </summary>
        /// <param name="applicationBuilder">
        /// The <see cref="IApplicationBuilder"/> which is used in the Http Pipeline
        /// </param>
        /// <param name="swaggerUrl">The URL for the Swagger endpoint</param>
        /// <param name="swaggerDescription">The description for the Swagger endpoint</param>
        public static void UseSwagger(this IApplicationBuilder applicationBuilder,
            string swaggerUrl, string swaggerDescription)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            applicationBuilder.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying
            // the Swagger JSON endpoint.
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerUrl, swaggerDescription);
            });
        }
    }
}