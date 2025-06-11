using Telegram.Bot.CarInsurance.Enums;

namespace Telegram.Bot.CarInsurance.Abstractions.Interfaces
{
    public interface IUserStateService
    {
        UserState GetUserState(long chatId);
        void SetState(long chatId, UserState state);
    }
}
