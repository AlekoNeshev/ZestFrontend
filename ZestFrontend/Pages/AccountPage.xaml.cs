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
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			nav.Paddings(0, 39);
			Grid.SetRow(nav, 1);
			MyGrid.Children.Add(nav);
		}
	}
}