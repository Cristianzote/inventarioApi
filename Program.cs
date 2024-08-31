using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using InventarioApi;
using inventarioApi.Data.Services;
using QuestPDF.Infrastructure;
using inventarioApi.Data.Models;
using Quartz;
using inventarioApi.Data.Jobs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SupabaseConnection");
builder.Services.AddDbContext<InventarioContext>(options => {
    options.UseNpgsql(connectionString);
});
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<RazorViewToStringRenderer>();
builder.Services.AddTransient<IMessage, Message >();
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));
builder.Services.AddScoped<Message>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<StatsService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<MonthlyRegisterService>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("RegisterMonthlyExpensesJob");
    q.AddJob<InventoryExpenseJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("RegisterMonthlyExpensesJob-trigger")
        .WithCronSchedule("0 0 1 19 * ?")); // s m h dia mes || 0 * * ? * * || 0 0 0 19 * ?
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

QuestPDF.Settings.License = LicenseType.Community;


//app.UseWelcomePage();

app.MapControllers();

app.Run();
