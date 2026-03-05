using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SSEStockPrice.Infrastructure.Data;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Infrastructure.ExternalApis;
using SSEStockPrice.Infrastructure.Repositories;
using Azure.Messaging.ServiceBus;
using SSEStockPrice.Infrastructure.Messaging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString")));


//Registering Service
builder.Services.AddHttpClient<IAlphaVantageClient, AlphaVantageClient>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddSingleton<ServiceBusClient>(new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnection")));
builder.Services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();



var host = builder.Build();
using(var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await host.RunAsync();
