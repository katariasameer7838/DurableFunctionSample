using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionNew
{
    public static class StarterFunction
    {
        [FunctionName("F_TaskExecutorStarter")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "task.start")]HttpRequest req, [DurableClient] IDurableOrchestrationClient starter, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            //Get generic parameters from query string
            string taskName = req.Query["taskName"];

            //Get request body
            string requestBody = new StreamReader(req.Body).ReadToEnd();

            //Create task info to pass along
            var taskInfo = new TaskInfo(taskName, requestBody, requestBody);

            //Start durable orchestration asynchronously
            var orchestrationId = starter.StartNewAsync("O_TaskExecutor", taskInfo).Result;

            //Return response with orchestration mgmt info
            return (ActionResult)new OkObjectResult(starter.CreateHttpManagementPayload(orchestrationId));
        }

    }
}
