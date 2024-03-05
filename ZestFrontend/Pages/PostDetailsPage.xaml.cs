using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class PostDetailsPage : ContentPage
{
	PostDetailsViewModel _viewModel;
	public PostDetailsPage(PostDetailsViewModel viewModel)
	{
		BindingContext = viewModel;
		this._viewModel = viewModel;
		InitializeComponent();
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		_viewModel.OnNavigatedTo();
		base.OnNavigatedTo(args);
	}
	/*protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		_viewModel.
		base.OnNavigatedFrom(args);
	}*/
}