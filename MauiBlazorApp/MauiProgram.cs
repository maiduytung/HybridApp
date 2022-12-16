using BlazorShared;
using BlazorShared.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace MauiBlazorApp
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
            builder.Services.AddMudServices();
            builder.Services.AddScoped<IProductService ,ProductService>();

            var sqlConnectionConfiguration = new SqlConnectionConfiguration("Server=192.168.50.212,1433;Database=KIEMKE;User ID=SA;Password=M@tkhau1;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;");
            builder.Services.AddSingleton(sqlConnectionConfiguration);

            return builder.Build();
        }
    }
}