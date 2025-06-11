using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.Abstractions.Abstract
{
    public abstract class CommandHandler
    {
        public async Task<CommandResult> HandleAsync(Message message)
        {
            //maybe cheak token 
            try
            {
                return await HandleInternalAsync(message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw new Exception("Abstract Point Error");
            }
        }
        protected abstract Task<CommandResult> HandleInternalAsync(Message message);
    }
}
