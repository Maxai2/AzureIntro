using System;
using System.IO;
using ConverterApp;
using ConverterApp.Interfaces;
using ConverterApp.Services;
using Microsoft.Extensions.Configuration;
using Refit;

namespace converterQueueMP
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();

            new Converter(
                audioExtractor: new AudioExtractor(),
                blobService: new BlobService(
                    connectionString: config["blob:connectionString"],
                    rootContainerName: config["blob:rootContainerName"]
                ),
                logger: new Serilogger(),
                tracksApi: RestService.For<ITracksApi>(config["api:url"]),
                serviceBusConnectionString: config["service:connectionString"],
                queueName: config["serviceBus:queue"]
            ).SubscribeAndListen().GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}
