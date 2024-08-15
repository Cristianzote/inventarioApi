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

            services.AddQuartz(q =>
            {

                var jobKey = new JobKey("RegisterMonthlyExpensesJob");

                q.AddJob<InventoryExpenseJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("RegisterMonthlyExpensesJob-trigger")
                    .WithCronSchedule("0 45 23 15 * ?")); // s m h d * cada mes
            });
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
