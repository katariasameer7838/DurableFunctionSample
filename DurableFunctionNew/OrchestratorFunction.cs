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

namespace DurableFunctionNew
{
    public static class OrchestratorFunction
    {
        [FunctionName("O_TaskExecutor")]
        public static async Task<object> TaskExecutor([OrchestrationTrigger]IDurableOrchestrationContext ctx, TraceWriter log)
        {
            var taskInfo = ctx.GetInput<TaskInfo>();

            if (ctx.IsReplaying == false)
                log.Info(String.Format("Starting activity {0}", taskInfo.Name));

            //Call first activity function dynamically
            var result = await ctx.CallActivityAsync<object>("A_SimulateLongRunningTask", taskInfo.Input);
            
            //Call next the perform callback activity function
            result = await ctx.CallActivityAsync<object>("A_PerformCallback", taskInfo.CallbackUrl);
            
            return true;
        }
    }
}
