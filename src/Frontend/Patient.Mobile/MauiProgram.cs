using LandaDoc.Frontend.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LandaDoc.Patient.Mobile;

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

		// ApiBaseUrls ships as a bundled MauiAsset (Resources/Raw/appsettings.json) rather than
		// wwwroot/appsettings.json, since MAUI Blazor Hybrid has no wwwroot-based config loader.
		using (var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").GetAwaiter().GetResult())
		{
			builder.Configuration.AddJsonStream(stream);
		}

		builder.Services.AddLandaDocFrontendShared(builder.Configuration);

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
