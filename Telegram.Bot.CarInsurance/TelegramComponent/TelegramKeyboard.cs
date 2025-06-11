using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.CarInsurance.TelegramComponent
{
    public class TelegramKeyboard : ITelegramKeyboard
    {

        public ReplyKeyboardMarkup Main()
        {
            var replayMarkup = new ReplyKeyboardMarkup(true);
            //replayMarkup.AddButton("Help");
            return replayMarkup;
        }
        public ReplyKeyboardMarkup YesNoButton()
        {
            var replayMarkup = new ReplyKeyboardMarkup(true);
            replayMarkup.AddButton("Yes").AddButton("No");
            return replayMarkup;
        }
    }
}
