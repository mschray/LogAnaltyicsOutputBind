using Microsoft.Azure.WebJobs;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace SampleExtension.Config
{
    internal class LogAnalyticsAsyncCollector : IAsyncCollector<LogAnalyticsMessage>
    {
        private LogAnalyticsConfiguration config;
        private LogAnalyticsAttribute attr;
        private static HttpClient client = new HttpClient();
        // You can use an optional field to specify the timestamp from the data. If the time field is not specified, Log Analytics assumes the time is the message ingestion time
        private static string TimeStampField = "";

        public LogAnalyticsAsyncCollector(LogAnalyticsConfiguration config, LogAnalyticsAttribute attr)
        {
            this.config = config;
            this.attr = attr;
        }
            
        public async Task AddAsync(LogAnalyticsMessage item, CancellationToken cancellationToken = default(CancellationToken))
        {
            var mergedItem = MergeMessageProperties(item, config, attr);
            await WriteLogEntry(mergedItem, attr);
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        // combine properties to create final message that will be sent
        private static LogAnalyticsMessage MergeMessageProperties(LogAnalyticsMessage item, LogAnalyticsConfiguration config, LogAnalyticsAttribute attr)
        {
            var result = new LogAnalyticsMessage
            {
                Text = FirstOrDefault(item.Text, attr.Text),
                CustomerId = FirstOrDefault(item.CustomerId, attr.CustomerID, config.CustomerID),
                SharedKey = FirstOrDefault(item.SharedKey, attr.SharedKey, config.SharedKey),
                LogName = FirstOrDefault(item.LogName, attr.LogName, config.LogName)
            };

            return result;
        }

        private static string FirstOrDefault(params string[] values)
        {
            return values.FirstOrDefault(v => !string.IsNullOrEmpty(v));
        }

        // Build the API signature
        public static string BuildSignature(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Send a request to the POST API endpoint
        public static async Task<string> PostData(string signature, string customerId, string logName, string date, string json)
        {

            string url = "https://" + customerId + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add("Log-Type", logName);
                client.Headers.Add("Authorization", signature);
                client.Headers.Add("x-ms-date", date);
                client.Headers.Add("time-generated-field", TimeStampField);
                return await client.UploadStringTaskAsync(new Uri(url), "POST", json);
            }

        }

        public static async Task<string> WriteLogEntry(LogAnalyticsMessage mergedItem, LogAnalyticsAttribute attribute)
        {
            string result = string.Empty;
            try
            {
                // Create a hash for the API signature
                var dateString = DateTime.UtcNow.ToString("r");
                string stringToHash = "POST\n" + mergedItem.Text.Length + "\napplication/json\n" + "x-ms-date:" + dateString + "\n/api/logs";


                string hashedString = BuildSignature(stringToHash, attribute.SharedKey);

                string signature = "SharedKey " + attribute.CustomerID + ":" + hashedString;
                
                result = await PostData(signature, attribute.CustomerID, attribute.LogName, dateString, mergedItem.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return result;
        }
       
    }
}
