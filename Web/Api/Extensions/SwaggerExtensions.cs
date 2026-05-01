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
                c.AddSecurityDefinition("X-User-Id", new OpenApiSecurityScheme
                {
                    Description = "User Id header",
                    Name = "X-User-Id",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // X-Roles header
                c.AddSecurityDefinition("X-User-Role", new OpenApiSecurityScheme
                {
                    Description = "Comma separated roles (e.g. Admin,User)",
                    Name = "X-User-Role",
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