using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.Object
{
    public class CommandResult
    {
        public Message Message { get; }
        public string? NextCommand { get; }

        private CommandResult(Message message, string? nextCommand)
        {
            this.Message = message;
            this.NextCommand = nextCommand;
        }
        public static CommandResult FromMessage(Message message)
        {
            return new CommandResult(message, null);
        }
        public static CommandResult FromNextCommand(string nextCommnad)
        {
            return new CommandResult(null!, nextCommnad);
        }
    }
}
