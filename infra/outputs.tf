output "resource_group_name" {
  value = azurerm_resource_group.pricetracker.name
}

output "sa_name" {
  value = azurerm_storage_account.pricetracker.name
}

output "asp_name" {
  value = azurerm_service_plan.pricetracker.name
}

output "fa_name" {
  value = azurerm_function_app_flex_consumption.pricetracker.name
}

output "fa_url" {
  value = "https://${azurerm_function_app_flex_consumption.pricetracker.name}.azurewebsites.net"
}

output "sql_connection_string" {
  value     = "Server=tcp:${azurerm_mssql_server.pricetracker.fully_qualified_domain_name},1433;Database=${azurerm_mssql_database.pricetracker.name};Authentication=Active Directory Default;"
  sensitive = true
}

output "key_vault_uri" {
  value = "azurerm_key_vault.pricetracker.vault_uri"
}

output "servicebus_namespace" {
  value = "azurerm_servicebus_namespace.pricetracker.name"
}

output "app_insights_name" {
  value = "azurerm_application_insights.pricetracker.name"
}