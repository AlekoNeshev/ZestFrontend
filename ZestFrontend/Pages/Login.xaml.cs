using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class Login : ContentPage
{
	public Login(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}