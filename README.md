# web-api-starter

Starter project for web api in .Net core. Below is a breakdown of the projects

## AzureTemplates

Contains the ARM templates that will need to be used to create the Azure resources

### web-api-template.json

    1. Description
        1. This template will create the web api infrastructure and all of it's related resources.
    2. Parameters
        1. **serverfarms_consumption_name** - "ar-scus-webapi-asp-dev" - App service plan to run the web app in
        2. **web_app_name** - "ar-scus-sales-dev" - Web App that will host the web api
        3. **app_insights_name** - "ar-scus-webapi-ai-dev" - App insights for the web api
        4. **enterprise_key_vault_name** - "ar-enterprise-kv-dev" - name of the key vault
    3. Deployment script

        Run this first if the resource group hasn't been created yet.

        ```bash
        user@Azure:~$ az group create --name <**ResourceGroupName**> --location <**AzureLocation**>
        ```

        ```bash
        user@Azure:~$ az group create -g ar-scus-webapi-rg-dev --location southcentralus
        user@Azure:~$ az deployment group create -g ar-scus-webapi-rg-dev --template-file web-api-template.json  --parameters web-api-template-dev.parameters.json
        ```

    4. Once the template is deployed you will need to allow the system identity that was created to have access to Azure Sql Server and the enterprise key vault.  Here are the steps to follow to do that.

       1. Updating the enterprise key vault
            1. Go to the enterprise key vault (in our case it is named ar-enterprise-kv-dev)
            2. Click on 'Access Policies' under the Settings blade
            3. Click '+ Add Access Policy', which will open up the Add access policy screen
                  1. Select the permissions you want or use a template
                  2. Select the principal (in this case it will be the principals for each of the functions)
                  3. Hit the Add button, then click the Save button
       2. Updating Azure Sql
            1. Login to the ODS database using an admin account and run the following script.  Note that the users you are creating are the same name as the Azure functions, this is the name of the managed identity in Azure AD.  If the identity is system-assigned it is always the same as the name of the App Service.  See <https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-connect-msi#grant-permissions-to-managed-identity> for more details on this.
            
            ```sql
            CREATE USER [ar-scus-sales-dev] FROM EXTERNAL PROVIDER;
            ALTER ROLE db_datareader ADD MEMBER [ar-scus-sales-dev];
            ALTER ROLE db_datawriter ADD MEMBER [ar-scus-sales-dev];
            GO
            ```

## Sales

This is the sales project that contains the web api controllers for Sales
