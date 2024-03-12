using CommunityToolkit.Maui.Markup;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class UserDetailsPage : ContentPage
{
	public UserDetailsPage(UserDetailsViewModel userDetailsViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = userDetailsViewModel;
		InitializeComponent();
		var nav = serviceProvider.GetRequiredService<NavigationView>();
		nav.Paddings(0, 74, 0, 0);
		MyGrid.Children.Add(nav);
	}
}