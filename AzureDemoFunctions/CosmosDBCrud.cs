using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace AzureDemoFunctions
{
    public static class CosmosDBCrud
    {
        [FunctionName("CosmosDBCrud")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "localy")] DocumentClient client,
            ILogger log)
        {
            string continuationToken = null;
            do
            {
                var feed = await client.ReadDocumentFeedAsync(
                    UriFactory.CreateDocumentCollectionUri("PersonAndDog", "CosmosContext"),
                    new FeedOptions { MaxItemCount = 10, RequestContinuation = continuationToken });
                continuationToken = feed.ResponseContinuation;
                foreach (Document document in feed)
                {
                    var dog = JsonConvert.DeserializeObject<Dog>(document.ToString());
                    if (dog.Name == "Rufus")
                    {
                        return (ActionResult)new OkObjectResult($"Hello {dog.Name}");
                    }
                }
            } while (continuationToken != null);
            return (ActionResult)new BadRequestObjectResult($"HittadeInteRufus");
        }
    }
}
