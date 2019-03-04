using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class QueueTriggerCSharp
    {
        [FunctionName("QueueTriggerCSharp")]
        public static void Run(
            [QueueTrigger("messages", Connection = "musicplayersg_STORAGE")]string myQueueItem, 
            [Blob("files/{rand-guid}.txt", FileAccess.Write, Connection="musicplayersg_STORAGE")] Stream blob,
            ILogger log)
        {
            var writer = new StreamWriter(blob);
            writer.Write(myQueueItem);
            writer.Close();
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
