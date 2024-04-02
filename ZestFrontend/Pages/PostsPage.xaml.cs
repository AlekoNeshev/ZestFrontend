
using MauiIcons.Core;
using ZestFrontend.ViewModels;
using ZestFrontend.CustomViews;

namespace ZestFrontend;

public partial class PostsPage : ContentPage
{
	private readonly PostsViewModel viewModel;

	public PostsPage(PostsViewModel viewModel, IServiceProvider serviceProvider)
	{
		
		InitializeComponent();
		if (Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.WinUI|| Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.macOS)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			Grid.SetRow(nav, 1);
			MyGrid.Children.Add(nav);
		}
		BindingContext = viewModel;
		this.viewModel = viewModel;
		
	}
	async protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		await this.viewModel.onNavigatedTo();
		base.OnNavigatedTo(args);
	}
	async protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		await viewModel.OnNavigatedFrom();
		base.OnNavigatedFrom(args);
	}

	private void ImageButton_Clicked(object sender, EventArgs e)
	{	
		if (SecondImageButton.Rotation == 0)
		{
			SecondImageButton.RotateTo(180);
		}
		else
		{
			SecondImageButton.RotateTo(0);
		}		
	}
	protected override void OnAppearing()
	{
		_ = new MauiIcon();
		base.OnAppearing();
	}
}