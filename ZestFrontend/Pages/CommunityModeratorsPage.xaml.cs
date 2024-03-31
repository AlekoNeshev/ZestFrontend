using CommunityToolkit.Maui.Markup;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend.Pages;

public partial class CommunityModeratorsPage : ContentPage
{
	public CommunityModeratorsPage(CommunityModeratorsViewModel viewModel, IServiceProvider serviceProvider)
	{
		BindingContext = viewModel;
		InitializeComponent();
		if (Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			nav.Paddings(0, 64);
			Grid.SetRow(nav, 1);
			MyGrid.Children.Add(nav);
		}
	}
}