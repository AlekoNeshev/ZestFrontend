using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

 
}

