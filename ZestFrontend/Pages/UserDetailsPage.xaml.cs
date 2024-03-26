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
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			MyGrid.Children.Add(nav);
		}
		}
	}