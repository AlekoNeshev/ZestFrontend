using ZestFrontend.ViewModels;

namespace ZestFrontend.Pages;

public partial class CommentDetailsPage : ContentPage
{
	public CommentDetailsPage(CommentDetailsViewModel commentDetailsViewModel)
	{
		BindingContext = commentDetailsViewModel;
		InitializeComponent();
	}
}