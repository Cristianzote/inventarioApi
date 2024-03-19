using inventarioApi.Data.Services;

namespace inventarioApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<InventoryService>();
            // Add other service registrations as needed
            services.AddControllers(); // Add MVC services
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Enable detailed error page in development
            }

            app.UseRouting(); // Enable routing

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Map controllers
            });
        }
    }
}
