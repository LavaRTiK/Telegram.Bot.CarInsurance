using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.CarInsurance.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.CommandHandlers
{
    public class InputPhotoCommandHandler : CommandHandler, ICommandHandler
    {
        private readonly TelegramBotClient _bot;
        private readonly ITelegramKeyboard _telegramKeyboard;
        private readonly ReadDocumentMindee _Mindee;
        private readonly IUserStateService _userStateService;
        private readonly IUserStateData _userStateData;

        public InputPhotoCommandHandler(TelegramBotClient bot, ReadDocumentMindee mindee, IUserStateService userStateService, IUserStateData userStateData, ITelegramKeyboard telegramKeyboard)
        {
            _bot = bot;
            _Mindee = mindee;
            _userStateService = userStateService;
            _userStateData = userStateData;
            _telegramKeyboard = telegramKeyboard;
        }
        public string Command => "InputPhoto";

        protected async override Task<CommandResult> HandleInternalAsync(Message message)
        {
            _userStateService.SetState(message.Chat.Id, Enums.UserState.InputPhoto);
            var reply = _telegramKeyboard.YesNoButton();
            var lastPhoto = message.Photo!.Last();
            string fileId = lastPhoto.FileId;
            await using var ms = new MemoryStream();
            var tgFile = await _bot.GetInfoAndDownloadFile(fileId, ms);
            var d = await _Mindee.ReadDriveLicense(ms);
            _userStateData.SetState(message.Chat.Id, d.Inference.Prediction.ToString());
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, $"Correct Data? \n\r {d.Inference.Prediction.ToString()}", replyMarkup:reply));
        }
    }
}
