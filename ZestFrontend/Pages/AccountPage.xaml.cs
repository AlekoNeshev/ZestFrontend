using CommunityToolkit.Maui.Markup;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountViewModel viewModel, IServiceProvider serviceProvider)
	{
		BindingContext = viewModel;
		InitializeComponent();
		var nav = serviceProvider.GetRequiredService<NavigationView>();
		nav.Paddings(0, 74, 0, 0);
		Grid.SetRow(nav, 1);
		MyGrid.Children.Add(nav);
	}
}