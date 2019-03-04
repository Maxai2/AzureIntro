using System.Collections.Generic;
using System.Linq;
using Company.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function {
    public static class CosmosDBTriggerCSharp {
        [FunctionName ("CosmosDBTriggerCSharp")]
        public static void Run ([CosmosDBTrigger (
                databaseName: "files",
                collectionName: "files",
                ConnectionStringSetting = "musicplayerdbcosmos_DOCUMENTDB",
                LeaseCollectionName = "leases",
                CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Document> inputt,
            ILogger log) {
            if (inputt != null && inputt.Count > 0) {
                //log.LogInformation ("Documents modified " + input);
                var input = inputt.Select(doc => JsonConvert.DeserializeObject<CustomFile>(doc.ToString())).ToList();
                log.LogInformation (input[0].BlobName);
                log.LogInformation (input[0].BlobLength.ToString ());
                log.LogInformation ("First document Id " + input[0].Id);
            }
        }
    }
}