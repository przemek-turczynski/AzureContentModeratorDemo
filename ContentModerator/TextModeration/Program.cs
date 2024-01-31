using Microsoft.Azure.CognitiveServices.ContentModerator;
using Newtonsoft.Json;
using System;
using System.Text;

namespace AzureAIServices.ContentModerator.TextModeration;

internal class Program
{
    private static readonly string SubscriptionKey = "51b47c50ae88477698364ed69b9ebeaa";

    private static readonly string Endpoint = "https://content-moderator-pt.cognitiveservices.azure.com/";

    static void Main()
    {
        // Create and configure client
        ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(SubscriptionKey));
        client.Endpoint = Endpoint;

        // Load the input text
        string text = File.ReadAllText("ExampleText.txt");

        // Remove carriage returns
        text = text.Replace(Environment.NewLine, " ");

        // Convert input text (string) to a byte[]
        // then into a stream (for parameter in ScreenText()).
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(text));

        // Moderate text
        var screenResult = client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);

        // Print result
        Console.WriteLine(JsonConvert.SerializeObject(screenResult, Formatting.Indented));
    }
}
