using Telegram.Bot.CarInsurance.Abstractions.Abstract;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.CarInsurance.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.CommandHandlers
{
    public class AnswerToUserCommandHandler : CommandHandler, ICommandHandler
    {
        private readonly IUserStateService _userStateService;

        private readonly IUserStateData _userStateData;
        private readonly TelegramBotClient _bot;
        private readonly OpenAIService _openAIService;
        public AnswerToUserCommandHandler(IUserStateService userStateService, IUserStateData userStateData, TelegramBotClient bot, OpenAIService openAIService)
        {
            _userStateService = userStateService;
            _userStateData = userStateData;
            _bot = bot;
            _openAIService = openAIService;
        }

        public string Command => "AnswerToUser";

        protected override async Task<CommandResult> HandleInternalAsync(Message message)
        {
            var currentUserState = _userStateService.GetUserState(message.Chat.Id);
            string startPromt = (currentUserState switch { 
                Enums.UserState.Main => "Info:the user is in the menu where you need to download the photo Indivication Passsport",
                Enums.UserState.InputPhotoC => "Info: the user is in a menu where he needs to say whether the data of Indivication Passport was calculated correctly or not",
                Enums.UserState.InputPhoto => "Info:The information user must upload his vehicle registration certificate in UA format.",
                Enums.UserState.InputPhoto2 => "Info: the user is in a menu where he needs to say whether the data of vehicle registration certificate in UA format was calculated correctly or not",
                Enums.UserState.GivePropositon => "Info: The user sees The fixed price of insurance is 100 US dollars. \\n \\r\\n Do you agree with the price?\"",
                Enums.UserState.LastPropositon => "Info:User see:We sincerely apologize — unfortunately, $100 USD is the only available price at this time \n\r\t \r\nDo you want to buy?"
            });
            string answer =await _openAIService.ReplyToTheUser(startPromt, message.Text);
            return CommandResult.FromMessage(await _bot.SendMessage(message.Chat.Id,$"[ASSISTENT]{answer}"));

        }
    }
}
