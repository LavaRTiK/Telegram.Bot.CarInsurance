using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.CarInsurance.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.CommandHandlers
{
    public class YesCommandHandler : CommandHandler, ICommandHandler
    {
        private readonly OpenAIService openAIService;
        private readonly ITelegramKeyboard _telegramKeyboard;
        private readonly IUserStateService _userStateService;
        private readonly TelegramBotClient _bot;
        private readonly IUserStateData _userStateData;

        public YesCommandHandler(IUserStateService userStateService, TelegramBotClient bot, OpenAIService openAIService, ITelegramKeyboard telegramKeyboard, IUserStateData userStateData)
        {
            _userStateService = userStateService;
            _bot = bot;
            this.openAIService = openAIService;
            _telegramKeyboard = telegramKeyboard;
            _userStateData = userStateData;
        }

        public string Command => "Yes";

        protected override async Task<CommandResult> HandleInternalAsync(Message message)
        {
            var currentState = _userStateService.GetUserState(message.Chat.Id);
            CommandResult commandResult = (currentState switch
            {
                UserState.InputPhoto => await GiveAPropositon(message),
                UserState.GivePropositon => await GenerateDocumentInsurance(message),
                _ => CommandResult.FromMessage(new Message())
            });
            return commandResult;
        }

        private async Task<CommandResult> GenerateDocumentInsurance(Message message)
        {
            string data = _userStateData.GetUserData(message.Chat.Id);
            var imgUri = await openAIService.GenerateInsurense(data);
            await _bot.SendPhoto(message.Chat.Id, InputFile.FromStream(imgUri.ImageBytes.ToStream()), caption: "You Insurance policy");
            var reply = _telegramKeyboard.Main();
            _userStateService.SetState(message.Chat.Id,UserState.Main);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id,"Go to Main",replyMarkup:reply));
        }

        private async Task<CommandResult> GiveAPropositon(Message message)
        {
            _userStateService.SetState(message.Chat.Id, UserState.GivePropositon);
            var replay = _telegramKeyboard.YesNoButton();
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "\r\nThe fixed price of insurance is 100 US dollars. \n \r\n Do you agree with the price?",replyMarkup: replay));
        }
    }
}
