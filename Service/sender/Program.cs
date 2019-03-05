using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace sender
{
    class Program
    {
        static void Main (string[] args) {
            var cstr = "Endpoint=sb://azerbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZlufGFq0isZbcr3rwUVLgvRaj4ItQn20fljXtGfKRvs=";
            var topic = "azertopic";
            var topicClient = new TopicClient (cstr, topic);
            var management = new ManagementClient (cstr);
            var name = Console.ReadLine ();
            if (!management.SubscriptionExistsAsync (topic, name).Result) {
                management.CreateSubscriptionAsync (topic, name).Wait ();
            }
            var subscriptionClient = new SubscriptionClient (cstr, topic, name);
            subscriptionClient.RegisterMessageHandler (OnMessage, OnException);
            while (true) {
                var text = Console.ReadLine ();
                if (text == "quit") break;
                var bytes = Encoding.Default.GetBytes (text);
                var msg = new Message (bytes) {
                    Label = name
                };
                topicClient.SendAsync (msg).Wait ();
            }
        }
        private static async Task OnException (ExceptionReceivedEventArgs e) {
            //System.Console.WriteLine(e.Exception.Message);
        }
        private static async Task OnMessage (Message msg, CancellationToken ct) {
            var text = Encoding.Default.GetString (msg.Body);
            System.Console.WriteLine ($"{msg.Label}: {text}");
        }
}
}
