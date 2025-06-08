using GymTracker.Pages;
using Microsoft.Extensions.Logging;

namespace GymTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<Database.AppDatabase>();
#if DEBUG
            builder.Logging.AddDebug();
#endif      
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();
            return builder.Build();
        }
    }
}
