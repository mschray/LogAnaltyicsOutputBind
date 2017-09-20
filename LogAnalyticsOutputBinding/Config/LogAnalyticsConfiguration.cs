// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SampleExtension.Config
{
    /// <summary>
    /// Extension for binding <see cref="LogAnalyticsMessage"/>.
    /// </summary>
    public class LogAnalyticsConfiguration : IExtensionConfigProvider
    {
        #region Global configuration defaults
        /// <summary>
        /// Gets or sets operations management workspace id used for log analytics messages.
        /// This value can be overridden by job functions.
        /// </summary>
        /// <remarks>
        /// This is used to write to correct Azure Log Analytics Workspaces
        /// </remarks>
        public string OperationsManagementWorkspace { get; set; }

        /// <summary>
        /// Gets or sets Operations Management Key to be used for log analytics messages.
        /// This value can be overridden by job functions.
        /// </summary>
        /// <remarks>
        /// This is the key used to authorize access to the Azure Log Analytics workspace
        /// </remarks>
        ///
        public string OperationsManagementKey { get; set; }

        /// <summary>
        /// Gets or sets the logname that will be used for Azure Log Analytics messages.
        /// This value can be overridden by job functions.
        /// </summary>
        /// <remarks>
        /// When you write to Azure Log Analytics you must name the log you are writing to and
        /// this is use for that purpose.
        /// </remarks>
        /// <example>
        /// Example of valid logname: MyLog
        /// </example>
        public string LogName { get; set; }
        #endregion

    public void Initialize(ExtensionConfigContext context)
    {
        // add converter between JObject and SlackMessage
        // Allows a user to bind to IAsyncCollector<JObject>, and the sdk will convert that to IAsyncCollector<SlackMessage>
        context.AddConverter<JObject, LogAnalyticsMessage>(input => input.ToObject<LogAnalyticsMessage>());

        // Add a binding rule for Collector
        context.AddBindingRule<LogAnalyticsAttribute>()
            .BindToCollector<LogAnalyticsMessage>(attr => new LogAnalyticsAsyncCollector(this, attr));
    }
    }
}
