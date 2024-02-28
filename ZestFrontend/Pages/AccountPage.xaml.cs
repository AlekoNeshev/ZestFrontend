using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}