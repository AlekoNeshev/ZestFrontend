using CommunityToolkit.Maui.Markup;
using ZestFrontend.ViewModels;
using ZestFrontend.CustomViews;

namespace ZestFrontend;

public partial class UserDetailsPage : ContentPage
{
	public UserDetailsPage(UserDetailsViewModel userDetailsViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = userDetailsViewModel;
		InitializeComponent();
		if (Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.WinUI|| Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.macOS)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			MyGrid.Children.Add(nav);
		}
		
	}
	void OnPointerEntered(object sender, PointerEventArgs e)
	{
		if (sender is View view)
		{
			// Change the color when the pointer enters the element
			view.BackgroundColor = Color.FromArgb("#7dd0ae"); // Change to your desired color
		}
	}

	void OnPointerExited(object sender, PointerEventArgs e)
	{
		if (sender is View view)
		{
			// Reset the color when the pointer exits the element
			view.BackgroundColor = Color.FromArgb("#f8f8ff");
		}
	}
}
