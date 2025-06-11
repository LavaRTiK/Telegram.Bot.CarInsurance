using Microsoft.AspNetCore.Components.Forms;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Images;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;
namespace Telegram.Bot.CarInsurance.Services
{
    public class OpenAIService
    {
        IConfiguration _configuration;
        private readonly string apiKey;
        public OpenAIService(IConfiguration configuration)
        {
            _configuration = configuration;
            apiKey = _configuration.GetValue<string>("OpenAI") != null ? _configuration.GetValue<string>("OpenAI") : Environment.GetEnvironmentVariable("OPEN_AI");
        }
        public async Task<GeneratedImage> GenerateInsurense(string data)
        {
            ImageClient client = new("dall-e-3", apiKey);
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
        public async Task<string> ReplyToTheUser(string currentState,string messageUser){
            ChatClient client = new(model: "gpt-3.5-turbo", apiKey: apiKey);
            var systemMessage = new SystemChatMessage("Your system prompt here...");
            var message = new List<ChatMessage>
            {
                new SystemChatMessage("You are an assistant in a Telegram chat. Your role is to support and inform users during their interaction with the insurance bot.\r\n\r\nYou will receive input in the following format:\r\n\"[Current bot screen or step] // [User's message]\"\r\n\r\nYour job is to:\r\n- Respond to general questions, small talk, or unrelated inquiries in a friendly and helpful way.\r\n- If the message is related to the insurance policy (e.g., prices, how to buy, what it includes), answer clearly and informatively.\r\n- If the message is completely unrelated to insurance or documents (e.g., jokes, off-topic questions), still try to respond.\r\n- **However**, if the user's message is clearly about documents or specific case files, or the bot step is unrelated to communication — reply with `\"skip\"` and nothing else.\r\n\r\nAdditionally:\r\n- Always end your reply with a reminder: \"You can continue purchasing the insurance policy at any time.\"\r\n- The insurance policy costs **$100** — if asked, respond with this amount.\r\n\r\nBe brief, helpful, and professional."),
                new UserChatMessage($"{currentState}//{messageUser}")
            };
            ChatCompletion completion = await client.CompleteChatAsync(message);
            return completion.Content[0].Text;

        }
    }
}
