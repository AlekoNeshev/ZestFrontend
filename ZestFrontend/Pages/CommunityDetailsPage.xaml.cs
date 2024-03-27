using CommunityToolkit.Maui.Markup;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class CommunityDetailsPage : ContentPage
{
	CommunityDetailsViewModel _viewModel;
	public CommunityDetailsPage(CommunityDetailsViewModel viewModel, IServiceProvider serviceProvider)
	{
		BindingContext = viewModel;
		_viewModel = viewModel;
		InitializeComponent();
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			nav.RowSpan(4);
			Grid.SetRow(nav, 1);
			MyGrid.Children.Add(nav);
		}
	}
	async protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		await this._viewModel.onNavigatedTo();
		base.OnNavigatedTo(args);
	}
	async protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		await _viewModel.OnNavigatedFrom();
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
}