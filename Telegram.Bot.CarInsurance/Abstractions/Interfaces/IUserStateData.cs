using Mindee.Parsing.Common;
using Mindee.Product.Generated;
using Mindee.Product.InternationalId;
using Telegram.Bot.CarInsurance.Enums;

namespace Telegram.Bot.CarInsurance.Abstractions.Interfaces
{
    public interface IUserStateData
    {
        Document<InternationalIdV2> GetUserInternationalIdV2(long chatId);
        void SetInternationalIdV2(long chatId, Document<InternationalIdV2> data);
        Document<GeneratedV1> GetUserTexPassport(long chatId);
        void SetUserTexPassport(long chatId, Document<GeneratedV1> data);
    }
}
