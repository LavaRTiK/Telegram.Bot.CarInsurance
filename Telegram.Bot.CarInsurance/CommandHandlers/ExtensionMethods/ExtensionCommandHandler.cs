using Telegram.Bot.CarInsurance.Abstractions.Interfaces;

namespace Telegram.Bot.CarInsurance.CommandHandlers.ExtensionMethods
{
    public static class ExtensionCommandHandler
    {
        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler, InputPhotoCommandHandler>();
            services.AddScoped<ICommandHandler, NoCommandHandler>();
            services.AddScoped<ICommandHandler, StartCommandHandler>();
            services.AddScoped<ICommandHandler, YesCommandHandler>();
        }
    }
}
