
data "azurerm_client_config" "current" {}


# Create a resource group
resource "azurerm_resource_group" "pricetracker" {
  location = var.resource_group_location
  name     = var.resource_group_name
}

# Random String for unique naming of resources
resource "random_string" "name" {
  length  = 8
  special = false
  upper   = false
  lower   = true
  numeric = false
}

# Create a storage account
resource "azurerm_storage_account" "pricetracker" {
  name                     = coalesce(var.sa_name, random_string.name.result)
  resource_group_name      = azurerm_resource_group.pricetracker.name
  location                 = azurerm_resource_group.pricetracker.location
  account_tier             = var.sa_account_tier
  account_replication_type = var.sa_account_replication_type
}

# Create a storage container
resource "azurerm_storage_container" "pricetracker" {
  name                  = "pricetracker-flexcontainer"
  storage_account_id    = azurerm_storage_account.pricetracker.id
  container_access_type = "private"
}

#Create Sql Server
resource "azurerm_mssql_server" "pricetracker" {
  name                         = coalesce(var.db_server_name, random_string.name.result)
  resource_group_name          = azurerm_resource_group.pricetracker.name
  location                     = azurerm_resource_group.pricetracker.location
  administrator_login          = "sqladmin"
  administrator_login_password = var.sql_admin_password
  version                      = "12.0"
}

#Create Database
resource "azurerm_mssql_database" "pricetracker" {
  name                         = coalesce(var.db_name, random_string.name.result)
  server_id                    = azurerm_mssql_server.pricetracker.id
  sku_name                     = "Basic"
  max_size_gb                  = 2
}


#Create Firewall rule
resource "azurerm_mssql_firewall_rule" "pricetracker" {
  name                         = coalesce(var.db_firewall_name, random_string.name.result)
  server_id                    = azurerm_mssql_server.pricetracker.id
  start_ip_address             = "0.0.0.0"
  end_ip_address               = "0.0.0.0"
}


# Create a Log Analytics workspace for Application Insights
resource "azurerm_log_analytics_workspace" "pricetracker" {
  name                = coalesce(var.ws_name, random_string.name.result)
  location            = azurerm_resource_group.pricetracker.location
  resource_group_name = azurerm_resource_group.pricetracker.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

# Create an Application Insights instance for monitoring
resource "azurerm_application_insights" "pricetracker" {
  name                = coalesce(var.ai_name, random_string.name.result)
  location            = azurerm_resource_group.pricetracker.location
  resource_group_name = azurerm_resource_group.pricetracker.name
  application_type    = "web"
  workspace_id = azurerm_log_analytics_workspace.pricetracker.id
}

# Create a service plan
resource "azurerm_service_plan" "pricetracker" {
  name                = coalesce(var.asp_name, random_string.name.result)
  resource_group_name = azurerm_resource_group.pricetracker.name
  location            = azurerm_resource_group.pricetracker.location
  sku_name            = "FC1"
  os_type             = "Linux"
}

# Create a function app
resource "azurerm_function_app_flex_consumption" "pricetracker" {
  name                = coalesce(var.fa_name, random_string.name.result)
  resource_group_name = azurerm_resource_group.pricetracker.name
  location            = azurerm_resource_group.pricetracker.location
  service_plan_id     = azurerm_service_plan.pricetracker.id

  storage_container_type      = "blobContainer"
  storage_container_endpoint  = "${azurerm_storage_account.pricetracker.primary_blob_endpoint}${azurerm_storage_container.pricetracker.name}"
  storage_authentication_type = "StorageAccountConnectionString"
  storage_access_key          = azurerm_storage_account.pricetracker.primary_access_key
  runtime_name                = var.runtime_name
  runtime_version             = var.runtime_version 
  maximum_instance_count      = 50
  instance_memory_in_mb       = 2048
  app_settings                = {
  APPLICATIONINSIGHTS_CONNECTION_STRING = azurerm_application_insights.pricetracker.connection_string
  }
  identity {
    type = "SystemAssigned"
  }

  site_config {
  }
}



# Create a service bus
resource "azurerm_servicebus_namespace" "pricetracker" {
  name                = coalesce(var.sb_name, random_string.name.result)
  resource_group_name = azurerm_resource_group.pricetracker.name
  location            = azurerm_resource_group.pricetracker.location
  sku                 = "Standard"
}

# Create Queue Price Ingested
resource "azurerm_servicebus_queue" "price_ingested" {
  name                                 = "sse-price-ingested"
  namespace_id                         = azurerm_servicebus_namespace.pricetracker.id
  max_delivery_count                   = 5
  dead_lettering_on_message_expiration = true
  lock_duration                        = "PT1M"
}

# Create Queue Price Alert
resource "azurerm_servicebus_queue" "price_alert" {
  name                                 = "sse-price-alert"
  namespace_id                         = azurerm_servicebus_namespace.pricetracker.id
  max_delivery_count                   = 3
  dead_lettering_on_message_expiration = true
  lock_duration                        = "PT1M"
}

# Create Queue Price Alert
resource "azurerm_servicebus_queue" "dividend_reminder" {
  name                                 = "sse-dividend-reminder"
  namespace_id                         = azurerm_servicebus_namespace.pricetracker.id
  max_delivery_count                   = 3
  dead_lettering_on_message_expiration = true
  lock_duration                        = "PT1M"
}

# Create a Key Vault
resource "azurerm_key_vault" "pricetracker" {
  name                       = coalesce(var.kv_name, random_string.name.result)
  resource_group_name        = azurerm_resource_group.pricetracker.name
  location                   = azurerm_resource_group.pricetracker.location
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"
  soft_delete_retention_days = 7
  purge_protection_enabled   = false
  access_policy {
  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = data.azurerm_client_config.current.object_id

  secret_permissions = ["Get", "List", "Set", "Delete", "Purge"]
}
}

# Create Access policy for Function App
resource "azurerm_key_vault_access_policy" "price_alert" {
  key_vault_id       = azurerm_key_vault.pricetracker.id
  tenant_id          = data.azurerm_client_config.current.tenant_id
  object_id          = azurerm_function_app_flex_consumption.pricetracker.identity[0].principal_id
  secret_permissions = ["Get", "List"]
}

# Create Secrets for Function App
resource "azurerm_key_vault_secret" "SqlAdminPassword" {
  name          = "SqlAdminPassword"
  value         = var.sql_admin_password
  key_vault_id  = azurerm_key_vault.pricetracker.id
}

# Create Secrets for Alpha Vantage Api
resource "azurerm_key_vault_secret" "AlphaVantageApi" {
  name          = "AlphaVantageApi"
  value         = var.alpha_vantage_api_key
  key_vault_id  = azurerm_key_vault.pricetracker.id
}

