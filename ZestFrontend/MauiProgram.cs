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
		builder.Services.AddSingleton<AuthService>();
		builder.Services.AddSingleton<AccountService>();
	    builder.Services.AddSingleton<LikesService>();

        builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<PostsViewModel>();
		builder.Services.AddSingleton<CommunitesViewModel>();
		builder.Services.AddSingleton<CommunityDetailsViewModel>();
		builder.Services.AddSingleton<AccountViewModel>();
		builder.Services.AddSingleton<PostDetailsViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<Login>();
		builder.Services.AddSingleton<PostsPage>();
		builder.Services.AddSingleton<CommunitiesPage>();
		builder.Services.AddSingleton<CommunityDetailsPage>();
		builder.Services.AddSingleton<AccountPage>();
		builder.Services.AddSingleton<PostDetailsPage>();


        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        return builder.Build();
	}
}
