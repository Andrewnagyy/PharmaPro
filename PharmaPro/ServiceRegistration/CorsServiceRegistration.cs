namespace PharmaPro.ServiceRegistration
{
    public static class CorsServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("ReactPolicy", build =>
            {
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            return services;
        }
    }
}
