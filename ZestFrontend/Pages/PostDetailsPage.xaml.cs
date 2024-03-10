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
		var nav = serviceProvider.GetRequiredService<NavigationView>();
		MyGrid.Children.Add(nav);
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		_viewModel.OnNavigatedTo();
		base.OnNavigatedTo(args);
	}
	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		_viewModel.OnNavigatedFrom();
		base.OnNavigatedFrom(args);
	}

}