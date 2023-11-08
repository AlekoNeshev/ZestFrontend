using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class CommunitiesPage : ContentPage
{
	public CommunitiesPage(CommunitesViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

}