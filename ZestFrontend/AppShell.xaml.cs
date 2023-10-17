namespace ZestFrontend;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(Login), typeof(Login));
		Routing.RegisterRoute(nameof(PostsPage), typeof(PostsPage));
    }
}
