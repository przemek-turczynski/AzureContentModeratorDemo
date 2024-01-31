using Azure.AI.ContentSafety;
using Azure;

namespace AzureAIServices.ContentSafety.TextModeration;

internal class Program
{
    private static readonly string SubscriptionKey = "4796b26a386c4ae08125c0f88bdde817";

    private static readonly string Endpoint = "https://content-safety-pt.cognitiveservices.azure.com/";

    static void Main()
    {
        // Create client
        ContentSafetyClient client = new ContentSafetyClient(new Uri(Endpoint), new AzureKeyCredential(SubscriptionKey));

        foreach (var imagePath in File.ReadAllLines("./samples/index.txt"))
        {
            EvaluateImage(client, $"./samples/{imagePath}");
        }
    }

    private static void EvaluateImage(ContentSafetyClient client, string imagePath)
    {
        Console.WriteLine("\n\nEvaluating {0}...", imagePath);

        ContentSafetyImageData image = new ContentSafetyImageData(BinaryData.FromBytes(File.ReadAllBytes(imagePath)));

        var request = new AnalyzeImageOptions(image);

        Response<AnalyzeImageResult> response;
        try
        {
            response = client.AnalyzeImage(request);
        }
        catch (RequestFailedException ex)
        {
            Console.WriteLine("Analyze image failed.\nStatus code: {0}, Error code: {1}, Error message: {2}", ex.Status, ex.ErrorCode, ex.Message);
            throw;
        }

        // Print result
        foreach (var category in response.Value.CategoriesAnalysis)
        {
            Console.WriteLine($"{category.Category}: {category.Severity}");
        }
    }
}
