using inventarioApi.Data.Services;
using Microsoft.Extensions.FileProviders;
using Quartz;
using Quartz.Spi;
using Quartz.Impl;
using inventarioApi.Data.Jobs;

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
            services.AddControllers();
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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Views")),
                RequestPath = "/views"
            });
        }
    }
}
