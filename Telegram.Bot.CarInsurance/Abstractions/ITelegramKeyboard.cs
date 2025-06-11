using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.CarInsurance.Abstractions
{
    public interface ITelegramKeyboard
    {
        ///<summary>
        ///Generate Keyboard Main
        ///</summary>
        ///<returns>Return object <see cref="ReplyKeyboardMarkup"/></returns>
        public ReplyKeyboardMarkup Main();
        ///<summary>
        ///Generate Keyboard YesNoButton
        ///</summary>
        ///<returns>Return object <see cref="ReplyKeyboardMarkup"/></returns>
        public ReplyKeyboardMarkup YesNoButton();
    }
}
