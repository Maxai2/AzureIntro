using System;
using System.IO;
using ConverterApp.Interfaces;
using ConverterApp.Services;
using Microsoft.Extensions.Configuration;
using Refit;

namespace ConverterApp {
    class Program {
        static void Main (string[] args) {
            var config = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json", true, true)
                .Build ();
            System.Console.WriteLine(config["blob:connectionString"]);
            new Converter (
                audioExtractor: new AudioExtractor (),
                blobService: new BlobService (
                    connectionString : config["blob:connectionString"],
                    rootContainerName : config["blob:rootContainerName"]
                ),
                logger : new Serilogger (),
                tracksApi : RestService.For<ITracksApi> (config["api:url"]),
                serviceBusConnectionString : config["serviceBus:connectionString"],
                queueName : config["serviceBus:queueName"]
            ).SubscribeAndListen ().GetAwaiter ().GetResult ();
            Console.ReadLine();
        }
    }
}