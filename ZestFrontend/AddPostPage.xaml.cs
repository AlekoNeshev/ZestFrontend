using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class AddPostPage : ContentPage
{
	public AddPostPage(AddPostViewModel addPostViewModel)
	{
		BindingContext = addPostViewModel;
		InitializeComponent();
	}
}