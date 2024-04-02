using ZestFrontend.ViewModels;
using ZestFrontend.CustomViews;

namespace ZestFrontend;

public partial class UsersPage : ContentPage
{
	UsersViewModel viewModel;
	public UsersPage(UsersViewModel usersViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = usersViewModel;
		this.viewModel = usersViewModel;
		InitializeComponent();
		if (Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.WinUI || Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.macOS)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			Grid.SetRow(nav, 1);
			MyGrid.Children.Add(nav);
		}
	}
	
}