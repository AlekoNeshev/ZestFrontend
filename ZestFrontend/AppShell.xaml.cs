using ZestFrontend.Pages;

namespace ZestFrontend;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(Login), typeof(Login));
        Routing.RegisterRoute(nameof(PostsPage), typeof(PostsPage));
        Routing.RegisterRoute(nameof(CommunitiesPage), typeof(CommunitiesPage));
        Routing.RegisterRoute(nameof(CommunityDetailsPage), typeof(CommunityDetailsPage));
        Routing.RegisterRoute(nameof(PostsPage), typeof(PostsPage));
        Routing.RegisterRoute(nameof(PostDetailsPage), typeof(PostDetailsPage));
		Routing.RegisterRoute(nameof(AddPostPage), typeof(AddPostPage));
		Routing.RegisterRoute(nameof(FriendsPage), typeof(FriendsPage));
		Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
		Routing.RegisterRoute(nameof(RegisterNewUser), typeof(RegisterNewUser));
		Routing.RegisterRoute(nameof(UsersPage), typeof(UsersPage));
		Routing.RegisterRoute(nameof(UserDetailsPage), typeof(UserDetailsPage));
		Routing.RegisterRoute(nameof(AddCommunityPage), typeof(AddCommunityPage));
		Routing.RegisterRoute(nameof(CommunityModeratorsPage), typeof(CommunityModeratorsPage));
	}
}
