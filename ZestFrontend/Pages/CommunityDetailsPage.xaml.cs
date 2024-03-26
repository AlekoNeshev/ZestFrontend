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
	private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
	{
	
    }
}