using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class RegisterNewUser : ContentPage
{
	public RegisterNewUser(RegisterNewUserViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}