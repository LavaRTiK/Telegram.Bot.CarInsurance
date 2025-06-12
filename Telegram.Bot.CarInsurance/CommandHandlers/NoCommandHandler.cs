using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.CarInsurance.UserService;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.CommandHandlers
{
    public class NoCommandHandler : CommandHandler, ICommandHandler
    {
        private readonly IUserStateService _userStateService;
        private readonly ITelegramKeyboard _telegramKeyboard;
        private readonly TelegramBotClient _bot;

        public NoCommandHandler(IUserStateService userStateService, TelegramBotClient bot, ITelegramKeyboard telegramKeyboard)
        {
            _userStateService = userStateService;
            _bot = bot;
            _telegramKeyboard = telegramKeyboard;
        }

        public string Command => "No";

        protected override async Task<CommandResult> HandleInternalAsync(Message message)
        {
            var current = _userStateService.GetUserState(message.Chat.Id);
            CommandResult commandResult = (current switch
            {
                Enums.UserState.InputPhoto => await RepeatInput(message),
                UserState.InputPhoto2 => await RepeatInput2(message),
                Enums.UserState.GivePropositon => await InteractiveDialoge(message),
                UserState.LastPropositon => await ToMain(message),
                _ => CommandResult.FromMessage(new Message())
            });
            return commandResult;
        }

        private async Task<CommandResult> RepeatInput2(Message message)
        {
            _userStateService.SetState(message.Chat.Id, UserState.InputPhoto);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "Upload a photo of your texPasport"));
        }

        private async Task<CommandResult> ToMain(Message message)
        {
            var reply = _telegramKeyboard.Main();
            _userStateService.SetState(message.Chat.Id, UserState.Main);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "Go to Main", replyMarkup: reply));
        }

        private async Task<CommandResult> InteractiveDialoge(Message message)
        {
            _userStateService.SetState(message.Chat.Id, UserState.LastPropositon);
            var reply = _telegramKeyboard.YesNoButton();
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "We sincerely apologize — unfortunately, $100 USD is the only available price at this time \n\r\t \r\nDo you want to buy?(yes/no)", replyMarkup:reply));
        }

        private async Task<CommandResult> RepeatInput(Message message)
        {
            _userStateService.SetState(message.Chat.Id, UserState.Main);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "Upload a photo of your passport"));
        }
    }
}
