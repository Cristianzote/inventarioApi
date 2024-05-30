using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using InventarioApi;
using inventarioApi.Data.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var connectionString = builder.Configuration.GetConnectionString("SupabaseConnection") ?? throw new ArgumentNullException("No tiene string de conexión");
var connectionString = "Host=aws-0-us-east-1.pooler.supabase.com;port=5432;Database=postgres;Username=postgres.uurvbhvptwrwyipgnkoe;Password=SfjI1S8bGUDcKTNU";
Console.WriteLine(builder.Configuration.GetConnectionString("SupabaseConnection"));

builder.Services.AddDbContext<InventarioContext>(options => {
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseWelcomePage();

app.MapControllers();

app.Run();
