using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.CarInsurance.CommandHandlers
{
    public class PurchaseInsuranceCommandHandler : CommandHandler, ICommandHandler
    {
        private readonly ITelegramKeyboard _telegramKeyboard;
        private readonly TelegramBotClient _bot;
        private readonly IUserStateService _userState;

        public PurchaseInsuranceCommandHandler(TelegramBotClient bot, IUserStateService userState, ITelegramKeyboard telegramKeyboard)
        {
            _bot = bot;
            _userState = userState;
            _telegramKeyboard = telegramKeyboard;
        }

        public string Command => "Purchase car insurance";

        protected override async Task<CommandResult> HandleInternalAsync(Message message)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = _telegramKeyboard.InsertState();
            _userState.SetState(message.Chat.Id,UserState.PurchaseInsurance);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id,"\r\nUpload a photo of your vehicle",replyMarkup:replyKeyboardMarkup));

        }
    }
}
