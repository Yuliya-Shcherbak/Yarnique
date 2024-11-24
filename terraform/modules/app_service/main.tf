resource "azurerm_linux_web_app" "app_service" {
  name                = var.service_name
  resource_group_name = var.resource_group_name
  location            = var.resource_location
  service_plan_id     = var.service_plan_id

  identity {
    type = "SystemAssigned"
  }

  site_config {
    container_registry_use_managed_identity = "true"
  }

  app_settings = {
    "KeyVaultUri" = var.key_vault_uri
    "WEBSITES_PORT" = 8080
  }
}
