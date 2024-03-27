using CommunityToolkit.Maui.Markup;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class PostDetailsPage : ContentPage
{
	PostDetailsViewModel _viewModel;
	public PostDetailsPage(PostDetailsViewModel viewModel, IServiceProvider serviceProvider)
	{
		
		BindingContext = viewModel;
		this._viewModel = viewModel;
		InitializeComponent();
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();

			nav.Paddings(0, 64, 0, 0);

			MyGrid.Children.Add(nav);
		}
	}

	async protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		await _viewModel.OnNavigatedTo();
		base.OnNavigatedTo(args);
	}
	async protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		await _viewModel.OnNavigatedFrom();
		base.OnNavigatedFrom(args);
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		CommentEntry.Text = string.Empty;
	}
}