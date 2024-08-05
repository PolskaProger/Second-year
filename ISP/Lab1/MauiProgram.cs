using Microsoft.Extensions.Logging;
using Lab1.Services;
using SQLite;

namespace Lab1
{
    public static class MauiProgram
    {
        //public static IServiceCollection services = new ServiceCollection();
        //public static IDbService? dbService = new SQLiteService();
        //public static IRateService? rateService = new RateService();
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
            builder.Services.AddHttpClient<IRateService, RateService>(client=>client.BaseAddress=new Uri("https://api.nbrb.by/exrates/rates"));
            builder.Services.AddTransient<IDbService, SQLiteService>();
            builder.Services.AddSingleton<Lab3Page>();
            //builder.Services.AddTransient<IRateService, RateService>();
            builder.Services.AddSingleton<Lab4Page>();
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
