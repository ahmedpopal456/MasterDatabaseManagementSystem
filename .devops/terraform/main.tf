resource "azurerm_resource_group" "main" {
  name     = "${var.organization_name}-${var.environment_name}-${var.service_abbreviation}"
  location = var.location_name
}

resource "azurerm_storage_account" "app" {
  for_each                 = toset(var.function_apps)
  name                     = "${var.organization_name}${var.environment_name}${var.service_abbreviation}${each.key}"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "main" {
  name                = "${var.organization_name}-${var.environment_name}-${var.service_abbreviation}-service-plan"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "main" {
  for_each                   = toset(var.function_apps)
  name                       = "${var.organization_name}-${var.environment_name}-${var.service_abbreviation}-${each.key}"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  app_service_plan_id        = azurerm_app_service_plan.main.id
  storage_account_name       = azurerm_storage_account.app[each.key].name
  storage_account_access_key = azurerm_storage_account.app[each.key].primary_access_key
  version = "~3"
  
  site_config {
    scm_type = "VSTSRM"
  }

  app_settings = {
    "AzureWebJobsStorage" : "UseDevelopmentStorage=false",
    "WEBSITE_ENABLE_SYNC_UPDATE_SITE" = "true",
    "WEBSITE_RUN_FROM_PACKAGE"        = "1",
    "APPINSIGHTS_INSTRUMENTATIONKEY"  = data.azurerm_application_insights.main.instrumentation_key,
    "WEBSITE_NODE_DEFAULT_VERSION"    = "10.14.1"
    "FUNCTIONS_WORKER_RUNTIME"        = "dotnet",

    "FIXIT-MDM-DB-CS" : data.azurerm_key_vault_secret.connectionstring.value,
  }
}
