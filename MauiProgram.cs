using Microsoft.Extensions.Logging;

namespace TodoListApp
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
                    fonts.AddFont("BlackOpsOne-Regular.ttf", "BlackOpsOne");
                    fonts.AddFont("LexendGiga-VariableFont_wght.ttf", "LexendGiga");
                    fonts.AddFont("SairaStencilOne-Regular.ttf", "SairaStencilOne");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
