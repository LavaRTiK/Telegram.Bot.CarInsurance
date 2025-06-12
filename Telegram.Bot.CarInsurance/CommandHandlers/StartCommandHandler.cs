using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.CarInsurance.CommandHandlers
{
    public class StartCommandHandler : CommandHandler, ICommandHandler
    {
        private readonly ITelegramKeyboard _telegramKeyboard;
        private readonly TelegramBotClient _bot;
        private readonly IUserStateService _userStateService;
        public string Command => "/start";
        public StartCommandHandler(TelegramBotClient bot, IUserStateService userStateService, ITelegramKeyboard telegramKeyboard)
        {
            _bot = bot;
            _userStateService = userStateService;
            _telegramKeyboard = telegramKeyboard;
        }
        protected override async Task<CommandResult> HandleInternalAsync(Message message)
        {
            _userStateService.SetState(message.Chat.Id, UserState.Main);
            ReplyKeyboardMarkup replyKeyboardMarkup = _telegramKeyboard.Main();
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "Hi, I'm a bot that will help with <b>Сar insurance.</b> \n\rUpload a photo of your passport ", Types.Enums.ParseMode.Html, replyMarkup: replyKeyboardMarkup));
        }
    }
}
