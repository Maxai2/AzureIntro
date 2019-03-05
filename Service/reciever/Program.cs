using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            var cstr = "Endpoint=sb://musicplayerserviceprice.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qTMNt1QDuDZ3mceg4U2svmoHJiXO5wopAw/qSlZj5mY=";
            var queue = "playerqueueSecond";
            var client = new QueueClient(cstr, queue);
            client.RegisterMessageHandler(OnMessage, OnException);
            Console.ReadLine();
        }

        private static async Task OnException(ExceptionReceivedEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        private static async Task OnMessage(Message msg, CancellationToken ct)
        {
            var text = Encoding.Default.GetString(msg.Body);
            Console.WriteLine(text + " " + msg.PartitionKey);
        }
    }
}
