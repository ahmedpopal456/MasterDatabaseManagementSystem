data "azurerm_key_vault" "main" {
    name = "${var.organization_name}-${var.environment_name}-${var.service_abbreviation}-kv"
    resource_group_name = azurerm_resource_group.main.name
}

data "azurerm_key_vault_secret" "connectionstring" {
  name         = "SQLDatabaseConnectionString"
  key_vault_id = data.azurerm_key_vault.main.id
}

data "azurerm_application_insights" "main" {
  name                = "${var.organization_name}-${var.environment_name}-common-ai"
  resource_group_name = "${var.organization_name}-${var.environment_name}-common"
}