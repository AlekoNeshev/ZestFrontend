using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class AddCommunityPage : ContentPage
{
	public AddCommunityPage(AddCommunityViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}