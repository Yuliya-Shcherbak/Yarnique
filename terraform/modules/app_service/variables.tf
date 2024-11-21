variable "service_name" {
  description = "Name of the App Service"
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

variable "image_name" {
  description = "Docker image tag"
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
