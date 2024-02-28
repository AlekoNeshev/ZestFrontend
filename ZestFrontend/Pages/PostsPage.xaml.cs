using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class PostsPage : ContentPage
{
	private readonly PostsViewModel viewModel;

	public PostsPage(PostsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		this.viewModel = viewModel;
	}
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{

		this.viewModel.onNavigatedTo();
		base.OnNavigatedTo(args);
	}

}