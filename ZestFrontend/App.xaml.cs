

namespace ZestFrontend;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

	//	AccountViewModel v = new AccountViewModel() { CreatedOn1 };

		MainPage = new AppShell();
	}
}
