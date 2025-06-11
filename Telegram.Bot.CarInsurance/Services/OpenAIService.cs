using OpenAI;
using OpenAI.Images;
using static System.Net.Mime.MediaTypeNames;
namespace Telegram.Bot.CarInsurance.Services
{
    public class OpenAIService
    {
        IConfiguration _configuration;
        public OpenAIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<GeneratedImage> GenerateInsurense(string data)
        {
            ImageClient client = new("dall-e-3", _configuration.GetValue<string>("OpenAI") != null ? _configuration.GetValue<string>("OpenAI") : Environment.GetEnvironmentVariable("OPEN_AI"));
            string prompt = $"Create an auto insurance policy for a customer.\r\nInclude the following details:\r\n– Customer full name\r\n– Address\r\n– Vehicle make, model, and year\r\n– Vehicle Identification Number (VIN)\r\n– Coverage types (e.g., liability, collision, comprehensive)\r\n– Policy start and end dates\r\n– Monthly premium amount\r\n– Deductibles for each coverage\r\n– Insurance provider name\r\n– Policy number\r\nFormat the policy professionally in clear and concise English.{data}";
            Console.WriteLine(prompt);

            ImageGenerationOptions options = new()
            {
                Quality = GeneratedImageQuality.High,
                Size = GeneratedImageSize.W1792xH1024,
                Style = GeneratedImageStyle.Vivid,
                ResponseFormat = GeneratedImageFormat.Bytes
            };
            GeneratedImage image = await client.GenerateImageAsync(prompt, options);
            BinaryData bytes = image.ImageBytes;
            return image;
        }
    }
}
