variable "resource_group_name" {
  type        = string
  default     = "price-tracker"
  description = "The name of the Azure resource group. If blank, a random name will be generated."
}

variable "resource_group_location" {
  type        = string
  default     = "uksouth"
  description = "Location of the resource group."
}

variable "sa_account_tier" {
  description = "The tier of the storage account. Possible values are Standard and Premium."
  type        = string
  default     = "Standard"
}

variable "sa_account_replication_type" {
  description = "The replication type of the storage account. Possible values are LRS, GRS, RAGRS, and ZRS."
  type        = string
  default     = "LRS"
}

variable "sa_name" {
  description = "The name of the storage account. If blank, a random name will be generated."
  type        = string
  default     = "pricetrackersa"
}

variable "db_name" {
  description = "The name of the database. If blank, a random name will be generated."
  type        = string
  default     = "sqldb-pricetracker"
}

variable "db_server_name" {
  description = "The name of the database server. If blank, a random name will be generated."
  type        = string
  default     = "sqlserver-pricetracker"
}

variable "db_firewall_name" {
  description = "The name of the db firewall rule server."
  type        = string
  default     = "AllowAzureServices"
}

variable "sql_admin_password" {
  description = "The SQL Server admin password."
  type        = string
  sensitive   = true
}

variable "alpha_vantage_api_key" {
  description = "The Alpha Vantage Api Key."
  type        = string
  sensitive   = true
}

variable "ws_name" {
  description = "The name of the Log Analytics workspace. If blank, a random name will be generated."
  type        = string
  default     = "logws"
}

variable "ai_name" {
  description = "The name of the Application Insights instance. If blank, a random name will be generated."
  type        = string
  default     = "price-tracker-appinsight"
}

variable "asp_name" {
  description = "The name of the App Service Plan. If blank, a random name will be generated."
  type        = string
  default     = "price-tracker-asp"
}

variable "fa_name" {
  description = "The name of the Function App. If blank, a random name will be generated."
  type        = string
  default     = "price-tracker-fa"
}


variable "sb_name" {
  description = "The name of Service Bus. If blank, a random name will be generated."
  type        = string
  default     = "pricetrackersb" 
}

variable "kv_name" {
  description = "The name of KeyVault. If blank, a random name will be generated."
  type        = string
  default     = "price-tracker-kv" 
}


variable "runtime_name" {
  description = "The name of the language worker runtime."
  type        = string
  default     = "dotnet-isolated" 
}

variable "runtime_version" {
  description = "The version of the language worker runtime."
  type        = string
  default     = "8.0" 
}

