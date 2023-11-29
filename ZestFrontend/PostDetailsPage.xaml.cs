using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class PostDetailsPage : ContentPage
{
	public PostDetailsPage(PostDetailsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}