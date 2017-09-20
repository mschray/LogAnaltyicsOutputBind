# Azure Long Analytics output binding sample

This sample is a Azure Functions binding extension that allows you to write a Azure Log Anayltics message by just adding an output binding.  What is Azure Log Anayltics?  [Centralize log data from multiple systems in a single data store.](https://azure.microsoft.com/en-us/services/log-analytics/).

## Using the binding

To use this binding you'll need a to add three settings to your local.settings.json with with the keys:

OperationsManagementWorkspace
OperationsManagementKey
LOG_ANALYTICS_APPNAME

## Where do I get the values for this setttings?
- The OPERATIONS_MANAGEMENT_WORKSPACE is the "name" (really a GUIDish thing) that is shows up under overview in the Azure Portal after you've created the Azure Log Analytics service. [Create your Azure Log Analytics Workspace](https://docs.microsoft.com/en-us/azure/log-analytics/log-analytics-get-started).
- The OPERATIONS_MANAGEMENT_KEY can be obtained the by looking at the data sources in this article.
- The LOG_ANALYTICS_APPNAME should be the name you want to appear for your app in the Log Analytics Service. Don't uses spaces of special characters.

### C#

Just reference the Log Analytics binding assembly and use the `[LogAnalytics]` attribute in your code:

```
csharp

    public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            [LogAnalytics(CustomerID = "CustomerID", SharedKey = "SharedKey", LogName = "LogName")] out LogAnalyticsMessage logAnalyticsMessage,
            TraceWriter log)
        {
```

### JavaScript

For JavaScript, the process is currently manual. Do the following:
1. Copy the extension to an output folder such as "extensions". This can be done in a post-build step in the .csproj
2. Add the app setting `AzureWebJobs_ExtensionsPath` to local.settings.json (or in Azure, in App Settings). Set the value to the **parent** of your "extension" folder from the previous step.

The project **SampleFunction** app already has a post-build step that copies the assembly to the folder **Extensions**.             