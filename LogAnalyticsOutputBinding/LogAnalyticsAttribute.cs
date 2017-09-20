﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using System;

namespace SampleExtension
{
    /// <summary>
    /// Attribute used to bind a parameter to a Slack. Message will be posted to Slack when the 
    /// method completes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public sealed class LogAnalyticsAttributeAttribute : Attribute
    {

        /// <summary>
        /// Sets the Text for the current outgoing Slack message. May include binding parameters.
        /// </summary>
        [AppSetting(Default = "CustomerID")]
        public string CustomerID { get; set; }

        /// <summary>
        /// Sets the Text for the current outgoing Slack message. May include binding parameters.
        /// </summary>
        [AppSetting(Default = "SharedKey")]
        public string SharedKey { get; set; }

        /// <summary>
        /// Sets the Text for the current outgoing Slack message. May include binding parameters.
        /// </summary>
        [AppSetting(Default = "LogName")]
        public string LogName { get; set; }

        /// <summary>
        /// Sets the Text for the current outgoing Slack message. May include binding parameters.
        /// </summary>
        [AutoResolve]
        public string Text { get; set; }
            
    }
}
