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
	async protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{

		await this.viewModel.onNavigatedTo();
		base.OnNavigatedTo(args);
	}

}