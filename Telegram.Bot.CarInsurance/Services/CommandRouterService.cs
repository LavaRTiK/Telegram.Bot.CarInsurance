using SkiaSharp;
using System;
using Telegram.Bot.CarInsurance.Abstractions;
using Telegram.Bot.CarInsurance.Abstractions.Interfaces;
using Telegram.Bot.CarInsurance.Enums;
using Telegram.Bot.CarInsurance.Object;
using Telegram.Bot.Types;

namespace Telegram.Bot.CarInsurance.Services
{
    public class CommandRouterService
    {
        private readonly IDictionary<string, ICommandHandler> _handlers;
        private readonly IUserStateService _userState;
        private readonly ITelegramKeyboard _telegramKeyboard;
        private readonly TelegramBotClient botClient;
        private readonly Dictionary<string, HashSet<UserState>> _allowedState = new()
        {
            //{"Menu", new HashSet<UserState>{UserState.Main} },
            //{"Main",new HashSet<UserState>{UserState.None,UserState.Menu,UserState.Main} },
        };
        public CommandRouterService(IEnumerable<ICommandHandler> handlers, IUserStateService userState, ITelegramKeyboard telegramKeyboard, TelegramBotClient botClient = null)
        {
            _handlers = handlers.ToDictionary(h => h.Command, StringComparer.OrdinalIgnoreCase);
            _userState = userState;
            _telegramKeyboard = telegramKeyboard;
            this.botClient = botClient;
        }
        public async Task<Message> RouteCommandAsync(Message message)
        {
            var currentState = _userState.GetUserState(message.Chat.Id);
            Console.WriteLine(currentState.ToString());
            long chatId = message.Chat.Id;
            string command = message.Text!;
            if(currentState == UserState.None && command != "/start")
            {
                _userState.SetState(chatId,UserState.Main);
                botClient.SendMessage(chatId, "to Main", replyMarkup: _telegramKeyboard.Main());
                return new Message();
            }
            if (message.Photo != null && currentState == UserState.PurchaseInsurance)
            {
                command = "InputPhoto";
            }
            if (string.IsNullOrWhiteSpace(command))
            {
                return new Message();
            }
            if (!_handlers.TryGetValue(command, out var handler))
            {
                return new Message();
            }
                if (_allowedState.TryGetValue(command, out var allowedState) && !allowedState.Contains(currentState))
            {
                throw new Exception("Denied acsses:" + command + "  currentUserState:" + currentState);
            }
            CommandResult commandResult = await handler.HandleAsync(message);
            while (!string.IsNullOrEmpty(commandResult.NextCommand))
            {
                if (!_handlers.TryGetValue(commandResult.NextCommand, out handler))
                {
                    throw new Exception($"Next command:'{commandResult.NextCommand}' not found");
                }
                commandResult = await handler.HandleAsync(message);
            }
            if (commandResult.Message is not null)
            {
                return commandResult.Message;
            }
            throw new Exception("Invalid command requst");
        }
    }
}
