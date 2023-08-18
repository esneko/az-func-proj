provider "azurerm" {
  features {}
}

resource "azurerm_linux_function_app" "example" {
  name                        = var.name
  location                    = var.location
  resource_group_name         = azurerm_resource_group.example.name
  service_plan_id             = azurerm_service_plan.example.id
  storage_account_name        = azurerm_storage_account.example.name
  storage_account_access_key  = azurerm_storage_account.example.primary_access_key
  functions_extension_version = "~4"
  https_only                  = true

  site_config {
    minimum_tls_version       = "1.2"
    http2_enabled             = true
    ftps_state                = var.ftps_state

    application_stack {
      dotnet_version          = "6.0"
    }
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME  = "dotnet"
  }

  depends_on = [
    azurerm_resource_group.example,
    azurerm_service_plan.example,
    azurerm_storage_account.example
  ]
}

resource "azurerm_resource_group" "example" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_service_plan" "example" {
  name                = var.service_plan_name
  location            = var.location
  resource_group_name = azurerm_resource_group.example.name
  os_type             = "Linux"
  sku_name            = "Y1"

  depends_on = [
    azurerm_resource_group.example
  ]
}

resource "azurerm_storage_account" "example" {
  name                     = var.storage_account_name
  location                 = var.location
  resource_group_name      = azurerm_resource_group.example.name
  account_tier             = "Standard"
  account_replication_type = "LRS"

  depends_on = [
    azurerm_resource_group.example
  ]
}
