using System.IO;
using Company.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class BlobTriggerCSharp
    {
        [FunctionName("BlobTriggerCSharp")]
        public static void Run(
            [BlobTrigger("files/{name}", Connection = "musicplayersg_STORAGE")]Stream myBlob, 
            string name, 
            [CosmosDB("files", "files", Id = "id", ConnectionStringSetting="musicplayerdbcosmos_DOCUMENTDB")]out CustomFile document,
            ILogger log)
        {
            document = new CustomFile{
                BlobName = name,
                BlobLength = myBlob.Length
            };
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
