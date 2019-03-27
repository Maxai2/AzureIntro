using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConverterApp;
using ConverterApp.Interfaces;
using ConverterApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using videoConverterWebJobMP.Utils;

namespace videoConverterWebJobMP
{
    public class Startup
    {
        public IConfiguration Configuration;
        AzureServiceTokenProvider _provider;
        KeyVaultClient _keyVaultClient;

        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
            _provider = new AzureServiceTokenProvider();
            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(_provider.KeyVaultTokenCallback));
        }

        private BlobOptions GetBlobString()
        {
            var blobSecretUrl = Configuration.GetSection("blob")["connectionString"];
            var blobSecret = _keyVaultClient.GetSecretAsync(blobSecretUrl).Result;

            string containerName = Configuration.GetSection("blob")["rootContainerName"];

            return new BlobOptions()
            {
                BlobSecret = blobSecret.Value,
                ContainerName = containerName
            };
        }

        private string GetApiUrl()
        {
            return Configuration.GetSection("api")["url"];
        }

        private ServiceBusOptions GetServiceBusString()
        {
            var serviceBusSecret = Configuration.GetSection("serviceBus")["connectionString"];
            var serviceBusString = _keyVaultClient.GetSecretAsync(serviceBusSecret).Result;

            var queueName = Configuration.GetSection("serviceBus")["queueName"];

            return new ServiceBusOptions()
            {
                ServiceBusSecret = serviceBusString.Value,
                QueueName = queueName
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.Run(async (context) =>
            // {
                
            // });
        }
    }
}
