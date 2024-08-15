using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using InventarioApi;
using inventarioApi.Data.Services;
using QuestPDF.Infrastructure;
using inventarioApi.Data.Models;
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
builder.Services.AddScoped <MonthlyRegisterService>();

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
