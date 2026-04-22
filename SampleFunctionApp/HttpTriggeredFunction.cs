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

            // 1. Try to get name from query parameter
            string name = req.Query["name"];

            // 2. If query param is empty, try reading from request body
            if (string.IsNullOrWhiteSpace(name))
            {
                using var reader = new StreamReader(req.Body);
                var requestBody = await reader.ReadToEndAsync();
    
                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    try
                    {
                        var json = JsonDocument.Parse(requestBody);
                        if (json.RootElement.TryGetProperty("name", out var nameProperty))
                        {
                            name = nameProperty.GetString();
                        }
                    }
                    catch (JsonException)
                    {
                        // Invalid JSON – ignore and fall through
                    }
                }
            }

            // 3. Final response
            if (!string.IsNullOrWhiteSpace(name))
            {
                return new OkObjectResult($"Hi {name}!");
            }

            return new OkObjectResult(
                "Welcome to Azure Functions! Please pass a name via query parameter or request body."
            );
        }
    }
}
