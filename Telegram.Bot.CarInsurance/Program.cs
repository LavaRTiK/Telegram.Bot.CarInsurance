using Mindee.Extensions.DependencyInjection;
using Telegram.Bot;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.CommandHandlers.ExtensionMethods;
using Telegram.Bot.CarInsurance.Services;
using Telegram.Bot.CarInsurance.TelegramComponent;
using Telegram.Bot.CarInsurance.UserService;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);
var token = builder.Configuration.GetValue<string>("BotToken") != null ? builder.Configuration.GetValue<string>("BotToken")! : Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
if (string.IsNullOrWhiteSpace(token))
{
    throw new InvalidOperationException("TelegramToken Empty");
}
var webhookUrl = builder.Configuration["BotWebhookUrl"]! == null ? Environment.GetEnvironmentVariable("BOT_WEB_HOOK") : builder.Configuration["BotWebhookUrl"];
builder.Services.AddHttpClient("tgwebhook").RemoveAllLoggers().AddTypedClient(httpClient => new TelegramBotClient(token, httpClient));
builder.Services.AddOpenApi();
//service
builder.Services.AddSingleton<IUserStateService, UserStateService>();
builder.Services.AddSingleton<IUserStateData, UserStateData>();
builder.Services.AddScoped<ITelegramKeyboard, TelegramKeyboard>();
//maybe singl
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddScoped<ReadDocumentMindee>();
//Extension command
builder.Services.AddCommandHandlers();
builder.Services.AddMindeeClient();
//Mindee__ApiKey
builder.Services.AddScoped<CommandRouterService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/bot/setWebhook", async (TelegramBotClient bot) => { await bot.SetWebhook(webhookUrl); return $"Webhook set to {webhookUrl}"; });
app.MapPost("/bot", OnUpdate);
app.MapGet(("/test"), () => $"Server Works! 0.0.1");
app.Run();
async Task OnUpdate(TelegramBotClient bot,Update update,CommandRouterService commandRouterService)
{
    if (update.Message is null) return;
    if (update.Message == null || (string.IsNullOrWhiteSpace(update.Message.Text) && update.Message.Photo == null))
        return;
    var msg = update.Message;
    Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");
    await commandRouterService.RouteCommandAsync(msg);
    // let's echo back received text in the chat
    //await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
}
