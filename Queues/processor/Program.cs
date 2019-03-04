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
                var msg = queue.GetMessageAsync().Result;
                queue.UpdateMessageAsync(
                    msg,
                    TimeSpan.FromMinutes(1),
                    MessageUpdateFields.Content | MessageUpdateFields.Visibility);
                if (msg == null)
                    continue;
                if (msg.DequeueCount > 5) {
                    queue.DeleteMessageAsync(msg).Wait();    
                    Console.WriteLine($"Msg {msg.Id} {msg.AsString} is invalid and was deleted!");
                }
                Console.WriteLine($"Msg {msg.Id} {msg.AsString} processed");
                queue.DeleteMessageAsync(msg).Wait();
            }
        }
    }
}
