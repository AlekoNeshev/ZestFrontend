using ZestFrontend.ViewModels;

namespace ZestFrontend.Pages;

public partial class CommunityModeratorsPage : ContentPage
{
	public CommunityModeratorsPage(CommunityModeratorsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}