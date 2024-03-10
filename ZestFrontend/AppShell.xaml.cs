using ZestFrontend.Pages;

namespace ZestFrontend;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
#if WINDOWS || MACCATALYST
        TabBarIsVisible = true;
#else
		TabBarIsVisible = false;
#endif

		BindingContext = this;
	
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
	public static readonly BindableProperty TabBarIsVisibleProperty =
	BindableProperty.Create(nameof(TabBarIsVisible), typeof(bool), typeof(AppShell), default(bool));

	public bool TabBarIsVisible
	{
		get => (bool)GetValue(TabBarIsVisibleProperty);
		set => SetValue(TabBarIsVisibleProperty, value);
	}
}
