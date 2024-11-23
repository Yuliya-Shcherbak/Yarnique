variable "acr_name" {
  description = "Azure Container Registry name"
  type        = string
}

variable "resource_group_name" {
  description = "The name of Azure Resource Group"
  type        = string
}

variable "resource_location" {
  description = "The location of Azure Resource Group"
  type        = string
}

variable "service_plan_id" {
  description = "Azure App Service Plan id"
  type        = string
}

variable "key_vault_uri" {
  description = "Azure Key Vault URL"
  type        = string
}
