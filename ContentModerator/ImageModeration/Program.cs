using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;

namespace AzureAIServices.ContentModerator.ImageModeration;

// Contains the image moderation results for an image, 
// including text and face detection results
public class EvaluationData
{
    // The URL of the evaluated image
    public string? ImageUrl {  get; set; }

    // The image moderation results
    public Evaluate? ImageModeration { get; set; }

    // The text detection results
    public OCR? TextDetection { get; set; }

    // The face detection results
    public FoundFaces? FaceDetection { get; set; }
}

internal class Program
{
    private static readonly string SubscriptionKey = "51b47c50ae88477698364ed69b9ebeaa";

    private static readonly string Endpoint = "https://content-moderator-pt.cognitiveservices.azure.com/";

    static void Main()
    {
        // Create and configure client
        ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(SubscriptionKey));
        client.Endpoint = Endpoint;

        // Store results
        List<EvaluationData> evaluationData = new List<EvaluationData>();

        // Analyze each image
        foreach(var imagePath in File.ReadAllLines("ImageUrls.txt"))
        {
            // Add results to Evaluation object
            evaluationData.Add(EvaluateImage(client, imagePath));
        }

        // Print results
        Console.WriteLine(JsonConvert.SerializeObject(evaluationData, Formatting.Indented));
    }

    private static EvaluationData EvaluateImage(ContentModeratorClient client, string imagePath)
    {
        Console.WriteLine("Evaluating {0}...", Path.GetFileName(imagePath));

        var imageUrl = new BodyModel("URL", imagePath.Trim());

        var imageData = new EvaluationData
        {
            ImageUrl = imageUrl.Value,

            // Evaluate for adult and racy content
            ImageModeration = client.ImageModeration.EvaluateUrlInput("application/json", imageUrl, true)
        };
        Thread.Sleep(1000);

        // Detect and extract text
        imageData.TextDetection = client.ImageModeration.OCRUrlInput("eng", "application/json", imageUrl, true);
        Thread.Sleep(1000);

        // Detect faces
        imageData.FaceDetection = client.ImageModeration.FindFacesUrlInput("application/json", imageUrl, true);
        Thread.Sleep(1000);

        return imageData;
    }
}