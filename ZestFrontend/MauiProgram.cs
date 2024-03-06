using MauiIcons.Core;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ZestFrontend.Services;
using ZestFrontend.ViewModels;
using MauiIcons.Fluent.Filled;
using ZestFrontend.Pages;

namespace ZestFrontend;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
		    .UseMauiCommunityToolkitMediaElement()
			.UseFluentFilledMauiIcons()
			.UseMauiCommunityToolkit()
			
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
		builder.Services.AddScoped<AuthService>();
		builder.Services.AddSingleton<AccountService>();
	    builder.Services.AddSingleton<LikesService>();
		builder.Services.AddSingleton<CommentService>();
		builder.Services.AddSingleton<FollowersService>();
		builder.Services.AddSingleton<MessageService>();
		builder.Services.AddSingleton<MediaService>();
		builder.Services.AddSingleton<LikesHubConnectionService>();
		builder.Services.AddSingleton<MessageHubConnectionService>();
		builder.Services.AddSingleton<CommentsHubConnectionService>();
		builder.Services.AddSingleton<SignalRConnectionService>();

        builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<PostsViewModel>();
		builder.Services.AddSingleton<CommunitesViewModel>();
		builder.Services.AddSingleton<CommunityDetailsViewModel>();
		builder.Services.AddSingleton<AccountViewModel>();
		builder.Services.AddTransient<PostDetailsViewModel>();
		builder.Services.AddSingleton<AddPostViewModel>();
		builder.Services.AddSingleton<FriendsViewModel>();
		builder.Services.AddSingleton<ChatViewModel>();
		builder.Services.AddSingleton<RegisterNewUserViewModel>();
		builder.Services.AddSingleton<UsersViewModel>();
		builder.Services.AddSingleton<UserDetailsViewModel>();
		builder.Services.AddSingleton<AddCommunityViewModel>();
		builder.Services.AddSingleton<CommunityModeratorsViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<Login>();
		builder.Services.AddSingleton<PostsPage>();
		builder.Services.AddSingleton<CommunitiesPage>();
		builder.Services.AddSingleton<CommunityDetailsPage>();
		builder.Services.AddSingleton<AccountPage>();
		builder.Services.AddSingleton<PostDetailsPage>();
		builder.Services.AddSingleton<AddPostPage>();
		builder.Services.AddSingleton<FriendsPage>();
		builder.Services.AddSingleton<ChatPage>();
		builder.Services.AddSingleton<RegisterNewUser>();
		builder.Services.AddSingleton<UsersPage>();
		builder.Services.AddSingleton<UserDetailsPage>();
		builder.Services.AddSingleton<AddCommunityPage>();
		builder.Services.AddSingleton<CommunityModeratorsPage>();

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        return builder.Build();
	}
}
