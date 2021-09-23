namespace DurableFunctionNew
{
    internal class TaskInfo
    {
        public string Name { get; set; }
        public string CallbackUrl { get; set; }
        public string Input { get; set; }

        public TaskInfo(string taskName, string callbackUrl, string requestBody)
        {
            this.Name = taskName;
            this.CallbackUrl = callbackUrl;
            this.Input = requestBody;
        }
    }
}