using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class FriendsPage : ContentPage
{
	public FriendsPage(FriendsViewModel friendsViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = friendsViewModel;
		InitializeComponent();
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			Grid.SetRow(nav, 1);
			MyGrid.Children.Add(nav);
		}
	}
}