using Auth0;
namespace ZestFrontend;

public partial class App : Application
{
	public App()
	{
#if WINDOWS
		if (Auth0.OidcClient.Platforms.Windows.Activator.Default.CheckRedirectionActivation())
			return;
#endif
		InitializeComponent();
		MainPage = new AppShell();
		Application.Current.UserAppTheme = AppTheme.Light;
	}

}
