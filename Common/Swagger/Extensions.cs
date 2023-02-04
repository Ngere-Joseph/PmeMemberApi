using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace PmeMemberApi.Common.Swagger
{
    public static class Extensions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Weather API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.CustomSchemaIds(x => x.FullName);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[]{}
                    }
                });

            });
            services.AddSwaggerGenNewtonsoftSupport();
            return services;
        }

        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder builder)
        {
            builder.UseSwagger(c => { c.RouteTemplate = "/_swagger/{documentName}/swagger.json"; });
            builder.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "_swagger";
                c.SwaggerEndpoint($"v1/swagger.json", $"weather API v1");
            });

            return builder;
        }
    }
}
