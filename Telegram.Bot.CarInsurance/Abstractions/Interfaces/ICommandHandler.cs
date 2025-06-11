using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.Abstractions.Interfaces
{
    public interface ICommandHandler
    {
        string Command { get; }
        public Task<CommandResult> HandleAsync(Message message);
    }
}
