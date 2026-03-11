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
using Azure.Communication.Email;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        Environment.GetEnvironmentVariable("SqlConnectionString"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null)));


//Registering Service
builder.Services.AddHttpClient<IAlphaVantageClient, AlphaVantageClient>();
builder.Services.AddScoped<IEmailService, AcsEmailService>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<ISentAlertRepository, SentAlertRepository>();
builder.Services.AddSingleton<ServiceBusClient>(new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnection")));
builder.Services.AddSingleton<EmailClient>(new EmailClient(Environment.GetEnvironmentVariable("AcsConnectionString")));
builder.Services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();



var host = builder.Build();
using(var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await host.RunAsync();
