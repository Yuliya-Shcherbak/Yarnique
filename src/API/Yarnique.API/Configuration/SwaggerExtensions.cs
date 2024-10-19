using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Yarnique.API.Configuration
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Yarnique API",
                    Version = "v1",
                    Description = "Pet project"
                });
                options.CustomSchemaIds(t => t.ToString());

            });

            return services;
        }

        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyMeetings API"); });

            return app;
        }
    }
}
