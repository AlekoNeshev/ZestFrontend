using Microsoft.Extensions.Logging;
using ZestFrontend.Services;
using ZestFrontend.ViewModels;

namespace ZestFrontend;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<HttpClient>();
		builder.Services.AddSingleton<LoginService>();
		builder.Services.AddSingleton<PostsService>();
		builder.Services.AddSingleton<CommunityService>();
        builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddSingleton<PostsViewModel>();
		builder.Services.AddSingleton<CommunitesViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<Login>();
		builder.Services.AddSingleton<PostsPage>();
		builder.Services.AddSingleton<CommunitiesPage>();


        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        return builder.Build();
	}
}
