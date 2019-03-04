using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace front
{
    class Program
    {
        static void Main(string[] args)
        {
            var cstr = "DefaultEndpointsProtocol=https;AccountName=musicplayersg;AccountKey=vAozpNUxpTgD+7w6MKhVjy02iDAwWIW7PLeTDmK71h9WxJLPSJafQcHm0e/raZhz0HQoccWNprqUDWhmuhb3Pw==;EndpointSuffix=core.windows.net";
            var account = CloudStorageAccount.Parse(cstr);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference("messages");
            queue.CreateIfNotExistsAsync().Wait();

            while (true) {
                var msg = Console.ReadLine();
                var qMsg = new CloudQueueMessage(msg);
                queue.AddMessageAsync(
                    qMsg,
                    TimeSpan.FromDays(7),
                    TimeSpan.FromSeconds(15),
                    new QueueRequestOptions(),
                    new OperationContext()).Wait();
            }
        }
    }
}
