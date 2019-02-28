using System.Collections.Generic;
using Company.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class CosmosDBTriggerCSharp
    {
        [FunctionName("CosmosDBTriggerCSharp")]
        public static void Run([CosmosDBTrigger(
            databaseName: "files",
            collectionName: "files",
            ConnectionStringSetting = "musicplayerdbcosmos_DOCUMENTDB",
            LeaseCollectionName = "leases", 
            CreateLeaseCollectionIfNotExists = true)]CustomFile input,
            ILogger log)
        {
            // if (input != null && input.Length > 0)
            // {
                log.LogInformation("Documents modified " + input);
                log.LogInformation(input.BlobName);
                log.LogInformation(input.BlobLength.ToString());
                log.LogInformation("First document Id " + input.Id);
            // }
        }
    }
}
