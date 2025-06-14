﻿using Mindee.Parsing.Common;
using Mindee.Product.Generated;
using Mindee.Product.InternationalId;
using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
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
            var userState = _userStateService.GetUserState(message.Chat.Id);
            if (UserState.Main == userState)
            {
                _userStateService.SetState(message.Chat.Id, Enums.UserState.InputPhotoC);
                var reply = _telegramKeyboard.YesNoButton();
                var lastPhoto = message.Photo!.Last();
                string fileId = lastPhoto.FileId;
                await using var ms = new MemoryStream();
                var tgFile = await _bot.GetInfoAndDownloadFile(fileId, ms);
                Document<InternationalIdV2> dataPassport = await _Mindee.ReadPassport(ms);
                _userStateData.SetInternationalIdV2(message.Chat.Id, dataPassport);
                var cutdata = dataPassport.Inference.Prediction;
                //сделать только корогтко имя и тд
                return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, $"Correct Data?(yes/no) \r\n FirstName:{cutdata.GivenNames.FirstOrDefault().Value} \n\r LastName:{cutdata.Surnames.FirstOrDefault().Value} \n\r", replyMarkup: reply));
            }
            else if (userState == UserState.InputPhoto)
            {
                _userStateService.SetState(message.Chat.Id, Enums.UserState.InputPhoto2);
                var reply = _telegramKeyboard.YesNoButton();
                var lastPhoto = message.Photo!.Last();
                string fileId = lastPhoto.FileId;
                await using var ms = new MemoryStream();
                var tgFile = await _bot.GetInfoAndDownloadFile(fileId, ms);
                Document<GeneratedV1> dataTex = await _Mindee.ReadTexPassport(ms);
                _userStateData.SetUserTexPassport(message.Chat.Id, dataTex);
                //сделать только корогтко имя и тд
                dataTex.Inference.Prediction.ToString();
                return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id, $"Correct Data?(yes/no) \r\n  Brand:{dataTex.Inference.Prediction.Fields.FirstOrDefault(n => n.Key == "brand").Value.ToString().Replace(":value:","").Replace("\n"," ").Replace("\r"," ").Trim()}  \r\nModel:{dataTex.Inference.Prediction.Fields.FirstOrDefault(x=>x.Key == "model").Value.ToString().Replace(":value:","").Replace("\n", " ").Replace("\r", " ").Trim()}", replyMarkup: reply));
            }
            return null;
        }
    }
}
