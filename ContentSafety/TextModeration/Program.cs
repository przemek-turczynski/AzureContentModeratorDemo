using Azure.AI.ContentSafety;
using Azure;
using Newtonsoft.Json;

namespace AzureAIServices.ContentSafety.ImageModeration;

internal class Program
{
    private static readonly string SubscriptionKey = "4796b26a386c4ae08125c0f88bdde817";

    private static readonly string Endpoint = "https://content-safety-pt.cognitiveservices.azure.com/";

    static void Main()
    {
        // Create client
        ContentSafetyClient client = new ContentSafetyClient(new Uri(Endpoint), new AzureKeyCredential(SubscriptionKey));

        // Load the input text
        string text = File.ReadAllText("ExampleText.txt");

        // Remove carriage returns
        text = text.Replace(Environment.NewLine, " ");

        var request = new AnalyzeTextOptions(text);

        Response<AnalyzeTextResult> response;
        try
        {
            response = client.AnalyzeText(request);
        }
        catch (RequestFailedException ex)
        {
            Console.WriteLine("Analyze text failed.\nStatus code: {0}, Error code: {1}, Error message: {2}", ex.Status, ex.ErrorCode, ex.Message);
            throw;
        }

        foreach(var category in response.Value.CategoriesAnalysis)
        {
            Console.WriteLine($"{category.Category}: {category.Severity}");
        }
    }
}
