using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.CarInsurance.TelegramComponent;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.CommandHandlers
{
    public class BackCommandHandler : CommandHandler, ICommandHandler
    {
        private readonly IUserStateService _userStateService;
        private readonly ITelegramKeyboard _telegramKeyboard;
        private readonly TelegramBotClient _bot;

        public BackCommandHandler(IUserStateService userStateService, TelegramBotClient bot, ITelegramKeyboard telegramKeyboard)
        {
            _userStateService = userStateService;
            _bot = bot;
            _telegramKeyboard = telegramKeyboard;
        }

        public string Command => "Back";

        protected override async Task<CommandResult> HandleInternalAsync(Message message)
        {
            CommandResult commandResult = (_userStateService.GetUserState(message.Chat.Id) switch
            {
                Enums.UserState.PurchaseInsurance => await GoMain(message),
                _ => CommandResult.FromMessage(new Message())
            });
            return commandResult;   
        }

        private async Task<CommandResult> GoMain(Message message)
        {
            var reply = _telegramKeyboard.Main();
            _userStateService.SetState(message.Chat.Id, UserState.Main);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "to Main", replyMarkup: reply));
        }
    }
}
