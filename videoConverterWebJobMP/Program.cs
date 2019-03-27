using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConverterApp;
using ConverterApp.Interfaces;
using ConverterApp.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using videoConverterWebJobMP.Utils;

namespace videoConverterWebJobMP {
    public class Program {
        static IConfiguration Configuration;
        static AzureServiceTokenProvider _provider;
        static KeyVaultClient _keyVaultClient;

        private static BlobOptions GetBlobString () {
            var blobSecretUrl = Configuration.GetSection ("blob") ["connectionString"];
            var blobSecret = _keyVaultClient.GetSecretAsync (blobSecretUrl).Result;

            string containerName = Configuration.GetSection ("blob") ["rootContainerName"];

            return new BlobOptions () {
                BlobSecret = blobSecret.Value,
                    ContainerName = containerName
            };
        }

        private static string GetApiUrl () {
            return Configuration.GetSection ("api") ["url"];
        }

        private static ServiceBusOptions GetServiceBusString () {
            var serviceBusSecret = Configuration.GetSection ("serviceBus") ["connectionString"];
            var serviceBusString = _keyVaultClient.GetSecretAsync (serviceBusSecret).Result;

            var queueName = Configuration.GetSection ("serviceBus") ["queueName"];

            return new ServiceBusOptions () {
                ServiceBusSecret = serviceBusString.Value,
                    QueueName = queueName
            };
        }

        public static void Main (string[] args) {
            Configuration = new ConfigurationBuilder ()
                .AddJsonFile ("appsettings.json", true)
                .AddJsonFile ("appsettings.development.json", true)
                .Build ();
            _provider = new AzureServiceTokenProvider ();
            _keyVaultClient = new KeyVaultClient (new KeyVaultClient.AuthenticationCallback (_provider.KeyVaultTokenCallback));

            var conv = new Converter (
                audioExtractor: new AudioExtractor (),
                blobService: new BlobService (
                    connectionString : GetBlobString ().BlobSecret,
                    rootContainerName : GetBlobString ().ContainerName
                ),
                logger : new Serilogger (),
                tracksApi : RestService.For<ITracksApi> (GetApiUrl ()),
                serviceBusConnectionString : GetServiceBusString ().ServiceBusSecret,
                queueName : GetServiceBusString ().QueueName
            );
            conv.SubscribeAndListen ();
            var host = CreateWebHostBuilder (args).Build ();
            host.Run ();
        }

        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseStartup<Startup> ();
    }
}