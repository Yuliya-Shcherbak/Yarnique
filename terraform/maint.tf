provider "azurerm" {
  features {}
}

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "yarnique-app"
    storage_account_name = "yarniquetfstatestorage"
    container_name       = "tfstate"
    key                  = "infrastructure.tfstate"
  }
}

module "yarnique_api_app_service" {
  source          = "./modules/app_service"
  service_name    = "yarnique-api"
  image_name      = "${var.acr_name}.azurecr.io/main-webapi:latest"
  service_plan_id = var.service_plan_id
  key_vault_uri   = var.key_vault_uri
  resource_group_name = var.resource_group_name
  resource_location = var.resource_location
}

module "payment_api_app_service" {
  source          = "./modules/app_service"
  service_name    = "payment-api"
  image_name      = "${var.acr_name}.azurecr.io/payment-api:latest"
  service_plan_id = var.service_plan_id
  key_vault_uri   = var.key_vault_uri
  resource_group_name = var.resource_group_name
  resource_location = var.resource_location
}

module "background_service_app_service" {
  source          = "./modules/app_service"
  service_name    = "background-service"
  image_name      = "${var.acr_name}.azurecr.io/background-service:latest"
  service_plan_id = var.service_plan_id
  key_vault_uri   = var.key_vault_uri
  resource_group_name = var.resource_group_name
  resource_location = var.resource_location
}
