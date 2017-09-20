using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using SampleExtension;

namespace SampleFunctionApp
{
    public static class HttpTriggerLog
    {
        [FunctionName("HttpTriggerLog")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            [LogAnalytics(CustomerID = "CustomerID", SharedKey = "SharedKey", LogName = "LogName")] out LogAnalyticsMessage logAnalyticsMessage,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            logAnalyticsMessage = new LogAnalyticsMessage();

            // parse query parameter
            string msg = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "msg", true) == 0)
                .Value;

            // because of the out keyword we can't make the method async so we'll do our async call this way
            // this is to get the req body incase it was passed in
            dynamic data = Task.Run(async () =>
            {
                return await req.Content.ReadAsAsync<object>();
            });

            // Set message to query string or body data
            string message = msg ?? data?.msg;

            // set the message to be logged
            logAnalyticsMessage.Text = message;

            return message == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a msg on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Message logged");

        }

    }

}