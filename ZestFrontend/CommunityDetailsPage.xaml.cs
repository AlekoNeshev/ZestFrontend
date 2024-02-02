using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class CommunityDetailsPage : ContentPage
{
	public CommunityDetailsPage(CommunityDetailsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}