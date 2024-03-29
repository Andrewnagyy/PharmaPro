using Microsoft.OpenApi.Models;

namespace PharmaPro.ServiceRegistration
{
    public static class SwaggerServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "PharmaPro Api",
                    Version = "v1"
                });
            });
            return services;
        }
    }
}
