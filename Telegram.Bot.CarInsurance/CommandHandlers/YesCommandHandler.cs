using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.CarInsurance.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
                UserState.InputPhotoC => await StartReadTex(message),
                UserState.InputPhoto2 => await GiveAPropositon(message),
                UserState.GivePropositon => await GenerateDocumentInsurance(message),
                _ => CommandResult.FromMessage(new Message())
            });
            return commandResult;
        }

        private async Task<CommandResult> StartReadTex(Message message)
        {
            _userStateService.SetState(message.Chat.Id,UserState.InputPhoto);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "Upload a photo of your technical passport(UA format)",replyMarkup: new ReplyKeyboardRemove()));
        }

        private async Task<CommandResult> GenerateDocumentInsurance(Message message)
        {
            var dataIndi = _userStateData.GetUserInternationalIdV2(message.Chat.Id).Inference.Prediction;
            var dataTex = _userStateData.GetUserTexPassport(message.Chat.Id).Inference.Prediction;
            //var imgUri = await openAIService.GenerateInsurense(dataIndiv.Prediction.ToString());
            //await _bot.SendPhoto(message.Chat.Id, InputFile.FromStream(imgUri.ImageBytes.ToStream()), caption: "You Insurance policy");
            await _bot.SendMessage(message.Chat.Id, $"🎉 Insurance successfully issued in the name {dataIndi.GivenNames.FirstOrDefault().Value} {dataIndi.Surnames.FirstOrDefault().Value} \r\nby car of Brand:{dataTex.Fields.FirstOrDefault(n=> n.Key == "brand").Value.ToString().Replace(":value:","").Replace("\n","").Replace("\r","").Trim()} Model:{dataTex.Fields.FirstOrDefault(n => n.Key == "model").Value.ToString().Replace(":value:","").Replace("\n", "").Replace("\r", "").Trim()} \r\nThank you for choosing our service. Drive safely! 🛡"); 
            _userStateService.SetState(message.Chat.Id,UserState.Main);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "Go to Main \r\nUpload a photo of your passport", replyMarkup: new ReplyKeyboardRemove()));
        }

        private async Task<CommandResult> GiveAPropositon(Message message)
        {
            _userStateService.SetState(message.Chat.Id, UserState.GivePropositon);
            var replay = _telegramKeyboard.YesNoButton();
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, "\r\nThe fixed price of insurance is 100 US dollars.\r\nDo you agree with the price?",replyMarkup: replay));
        }
    }
}
