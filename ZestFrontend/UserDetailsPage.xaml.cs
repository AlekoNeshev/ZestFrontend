using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class UserDetailsPage : ContentPage
{
	public UserDetailsPage(UserDetailsViewModel userDetailsViewModel)
	{
		BindingContext = userDetailsViewModel;
		InitializeComponent();
	}
}