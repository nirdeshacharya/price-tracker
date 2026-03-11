# Price Tracker

A cloud-native stock price tracking and alerting system built on Azure, targeting SSE PLC (SSE.LON) shares listed on the London Stock Exchange.
Built to demonstrate real-world Azure infrastructure, event-driven architecture, and .NET 8 serverless development patterns relevant to trading and finance environments.



## Infrastructure

Provisioned entirely with Terraform:

- Azure Function App (Flex Consumption, Linux, .NET 8 isolated)
- Azure SQL Server + Database (Basic tier)
- Azure Service Bus (Standard, two queues)
- Azure Key Vault (secrets management, managed identity access)
- Application Insights + Log Analytics Workspace (observability)
- Azure Storage Account (Function App runtime)
- Azure Communication Services + Email Domain

All application secrets are stored in Key Vault. 
Both the Function App uses system-assigned managed identity 
to access Key Vault at runtime — no credentials stored in 
application settings.


## Tech Stack

- .NET 8 isolated Azure Functions
- Entity Framework Core 8 (code first)
- Azure Service Bus
- Azure SQL
- Azure Communication Services
- Azure Key Vault
- Application Insights
- Terraform