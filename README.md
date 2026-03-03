# Price Tracker

A cloud-native commodity price tracking system built on Azure.
Ingests live commodity prices from Alpha Vantage on a scheduled basis,
stores them in Azure SQL, and publishes events to Azure Service Bus 
for downstream alerting workflows.

Built to demonstrate real-world Azure infrastructure and .NET 8 
serverless development patterns.



## Infrastructure

Provisioned entirely with Terraform:

- Azure Function App (Flex Consumption, Linux, .NET 8 isolated)
- Azure SQL Server + Database (Basic tier)
- Azure Service Bus (Standard, two queues)
- Azure Key Vault (secrets management, managed identity access)
- Application Insights + Log Analytics Workspace (observability)
- Azure Storage Account (Function App runtime)

All application secrets are stored in Key Vault. 
Both the Function App uses system-assigned managed identity 
to access Key Vault at runtime — no credentials stored in 
application settings.



## Deploying the Infrastructure

### Prerequisites
- Azure subscription
- Terraform >= 1.5
- Azure CLI
- Free Alpha Vantage API key: https://www.alphavantage.co/support/#api-key

### Steps

1. Clone the repo
   git clone https://github.com/nirdeshacharya/price-tracker.git
   cd price-tracker/infra

2. Create your tfvars file
   cp terraform.tfvars.example terraform.tfvars
   
   Fill in your values:
   - sql_admin_password
   - alpha_vantage_api_key

3. Deploy
   terraform init
   terraform plan
   terraform apply

4. Tear down when done
   terraform destroy


   ## Known Limitations and Future Improvements

- No VNet integration or Private Endpoint for SQL — 
  in production the database would sit behind a private endpoint 
  with public access disabled entirely
- No Web API layer yet — query endpoints for price history 
  and threshold management to be added
- No GitHub Actions CI/CD pipeline yet — planned next
- Terraform state is local — remote state via Azure Storage 
  backend is prepared in main.tf and ready to uncomment
