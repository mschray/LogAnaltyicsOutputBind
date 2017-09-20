using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleExtension
{
    public class LogAnalyticsMessage
    {

        // Update customerId to your Operations Management Suite workspace ID
        [JsonProperty("customerid")]
        public string CustomerId { get; set; }

        // For sharedKey, use either the primary or the secondary Connected Sources client authentication key   
        [JsonProperty("sharedkey")]
        public string SharedKey { get; set; }

        // LogName is name of the event type that is being submitted to Log Analytics
        [JsonProperty("logname")]
        public string LogName { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
        
        public override string ToString()
        {
            return $"{Text}";
        }
    }
}
