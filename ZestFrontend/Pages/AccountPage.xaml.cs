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
		
		Grid.SetRow(nav, 1);
		MyGrid.Children.Add(nav);
	}
}