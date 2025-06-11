using Mindee.Parsing.Common;
using Mindee.Product.Generated;
using Mindee.Product.InternationalId;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Telegram.Bot.CarInsurance.UserService
{
    public class UserStateData : IUserStateData
    {
        private readonly Dictionary<long, Document<InternationalIdV2>> _userDataInternationalId = new();
        private readonly Dictionary<long, Document<GeneratedV1>> _userDataTexPassport = new();
        public Document<InternationalIdV2> GetUserInternationalIdV2(long chatId)
        {
            if (_userDataInternationalId.TryGetValue(chatId, out Document<InternationalIdV2> data))
            {
                return data;
            }
            return null;
        }


        public void SetInternationalIdV2(long chatId, Document<InternationalIdV2> data)
        {
            _userDataInternationalId[chatId] = data;
        }
        public Document<GeneratedV1> GetUserTexPassport(long chatId)
        {
            if (_userDataTexPassport.TryGetValue(chatId, out Document<GeneratedV1> data))
            {
                return data;
            }
            return null;
        }

        public void SetUserTexPassport(long chatId, Document<GeneratedV1> data)
        {
            _userDataTexPassport[chatId] = data;
        }
    }
}
