using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureDemoFunctions
{
    public class Dog
    {
        public string Name { get; set; }
        public bool HasTail { get; set; }
        public int NumberOfLegs { get; set; }
    }
    public static class QueryParams
    {
        [FunctionName("QueryParams")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //Här är queryParams:
            //string name = req.Query["name"]

            //Här är request Body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Dog dog = JsonConvert.DeserializeObject<Dog>(requestBody);
            if (dog.Name != "Rufus")
            {
                return (ActionResult)new BadRequestObjectResult("NOT ALLOWED ACCESS");
            }
            
            return (ActionResult)new OkObjectResult($"Hello, your dog {dog.Name} is nice, Why does it have {dog.NumberOfLegs} legs?");
                
        }
    }
}
