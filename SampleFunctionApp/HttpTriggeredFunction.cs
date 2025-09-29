using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SampleFunctionApp
{
    public class HttpTriggeredFunction
    {
        private readonly ILogger<HttpTriggeredFunction> _logger;

        public HttpTriggeredFunction(ILogger<HttpTriggeredFunction> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggeredFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
