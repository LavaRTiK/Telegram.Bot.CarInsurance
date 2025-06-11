using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;

namespace Telegram.Bot.CarInsurance.UserService
{
    public class UserStateData : IUserStateData
    {
        private readonly Dictionary<long, string> _userData = new();
        public string GetUserData(long chatId)
        {
            if (_userData.TryGetValue(chatId, out string data))
            {
                return data;
            }
            return "none";
        }

        public void SetState(long chatId, string data)
        {
            _userData[chatId] = data;
        }
    }
}
