using Microsoft.OpenApi.Models;

namespace Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

                // X-UserId header
                c.AddSecurityDefinition("X-UserId", new OpenApiSecurityScheme
                {
                    Description = "User Id header",
                    Name = "X-UserId",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // X-Roles header
                c.AddSecurityDefinition("X-Roles", new OpenApiSecurityScheme
                {
                    Description = "Comma separated roles (e.g. Admin,User)",
                    Name = "X-Roles",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-UserId"
                            }
                        },
                        Array.Empty<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-Roles"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}