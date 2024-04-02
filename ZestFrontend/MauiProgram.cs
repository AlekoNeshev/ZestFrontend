using MauiIcons.Core;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ZestFrontend.Services;
using ZestFrontend.ViewModels;
using MauiIcons.Fluent.Filled;
using ZestFrontend.Pages;
using ZestFrontend.CustomViews;
using Material.Components.Maui.Extensions;
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
		builder.Services.AddSingleton<AuthService>();
		builder.Services.AddSingleton<AccountService>();
	    builder.Services.AddSingleton<LikesService>();
		builder.Services.AddSingleton<CommentService>();
		builder.Services.AddSingleton<FollowersService>();
		builder.Services.AddSingleton<MessageService>();
		builder.Services.AddSingleton<MediaService>();
		builder.Services.AddSingleton<LikesHubConnectionService>();
		builder.Services.AddSingleton<MessageHubConnectionService>();
		builder.Services.AddSingleton<DeleteHubConnectionService>();
		builder.Services.AddSingleton<SignalRConnectionService>();

        builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<PostsViewModel>();
		builder.Services.AddSingleton<CommunitesViewModel>();
		builder.Services.AddSingleton<CommunityDetailsViewModel>();
		builder.Services.AddSingleton<AccountViewModel>();
		builder.Services.AddSingleton<PostDetailsViewModel>();
		builder.Services.AddTransient<AddPostViewModel>();
		builder.Services.AddSingleton<FriendsViewModel>();
		builder.Services.AddSingleton<ChatViewModel>();
		builder.Services.AddSingleton<UsersViewModel>();
		builder.Services.AddSingleton<UserDetailsViewModel>();
		builder.Services.AddTransient<AddCommunityViewModel>();
		builder.Services.AddSingleton<CommunityModeratorsViewModel>();
		builder.Services.AddSingleton<NavigationViewModel>();
		builder.Services.AddTransient<CommentDetailsViewModel>();

        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<PostsPage>();
		builder.Services.AddSingleton<CommunitiesPage>();
		builder.Services.AddSingleton<CommunityDetailsPage>();
		builder.Services.AddSingleton<AccountPage>();
		builder.Services.AddSingleton<PostDetailsPage>();
		builder.Services.AddTransient<AddPostPage>();
		builder.Services.AddSingleton<FriendsPage>();
		builder.Services.AddSingleton<ChatPage>();
		builder.Services.AddSingleton<UsersPage>();
		builder.Services.AddSingleton<UserDetailsPage>();
		builder.Services.AddTransient<AddCommunityPage>();
		builder.Services.AddSingleton<CommunityModeratorsPage>();
		builder.Services.AddTransient<CommentDetailsPage>();

		builder.Services.AddTransient<NavigationView>();

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        return builder.Build();
	}
}
