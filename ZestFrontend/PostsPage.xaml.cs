using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class PostsPage : ContentPage
{
	public PostsPage(PostsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
	
}