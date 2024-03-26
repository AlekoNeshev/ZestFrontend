using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class AddCommunityPage : ContentPage
{
	public AddCommunityPage(AddCommunityViewModel viewModel, IServiceProvider serviceProvider)
	{
		BindingContext = viewModel;
		InitializeComponent();
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			MyGrid.Children.Add(nav);
		}
	}
}