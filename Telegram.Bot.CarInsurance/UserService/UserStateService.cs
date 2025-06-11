using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;

namespace Telegram.Bot.CarInsurance.UserService
{
    public class UserStateService : IUserStateService
    {
        private readonly Dictionary<long,UserState> _userState = new();
        public UserState GetUserState(long chatId)
        {
            if(_userState.TryGetValue(chatId,out UserState state))
            {
                return state;
            }
            return UserState.None;
        }
        public void SetState(long chatId, UserState state)
        {
            _userState[chatId] = state;
        }
    }
}
