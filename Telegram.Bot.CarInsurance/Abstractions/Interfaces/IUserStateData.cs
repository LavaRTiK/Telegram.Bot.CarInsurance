using Telegram.Bot.CarInsurance.Enums;

namespace Telegram.Bot.CarInsurance.Abstractions.Interfaces
{
    public interface IUserStateData
    {
        string GetUserData(long chatId);
        void SetState(long chatId, string data);
    }
}
