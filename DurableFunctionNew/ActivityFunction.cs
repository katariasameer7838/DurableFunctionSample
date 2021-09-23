using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Http;

namespace DurableFunctionNew
{
    public static class ActivityFunction
    {
        [FunctionName("A_SimulateLongRunningTask")]
        public static async Task<object> TaskExecutor([ActivityTrigger] string taskInput, TraceWriter log)
        {
            dynamic longRunningTask = JsonConvert.DeserializeObject(taskInput);

            //Simulate a long running task, based on the provided duration
            await Task.Delay(TimeSpan.FromSeconds((int)longRunningTask.taskDurationInSeconds));
            return true;
        }


        private static HttpClient httpClient = new HttpClient();

        [FunctionName("A_PerformCallback")]
        public static async Task<object> PerformCallback([ActivityTrigger] string taskInput, TraceWriter log)
        {
            dynamic longRunningTask = JsonConvert.DeserializeObject(taskInput);
            log.Info("CallBack URL = " + longRunningTask.callbackUrl);
            //Perform an HTTP post on the provided URL
            var request = new HttpRequestMessage(HttpMethod.Post, (string)longRunningTask.callbackUrl);
            request.Content = new StringContent("OK");
            var response = await httpClient.SendAsync(request);
            log.Info("callback response = " + response.StatusCode+"*****"+response.Content.ReadAsStringAsync());
            return true;
        }

    }
}
