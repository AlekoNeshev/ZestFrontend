using Auth0;
namespace ZestFrontend;

public partial class App : Application
{
	public App()
	{
		
		InitializeComponent();
		MainPage = new AppShell();
		Application.Current.UserAppTheme = AppTheme.Light;
	}

}
