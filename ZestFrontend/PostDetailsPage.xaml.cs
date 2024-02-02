using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class PostDetailsPage : ContentPage
{
	public PostDetailsPage(PostDetailsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	private void Frame_DescendantAdded(object sender, ElementEventArgs e)
	{

    }

	private void CollectionView_SizeChanged(object sender, EventArgs e)
	{

    }
}