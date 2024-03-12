using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class CommunitiesPage : ContentPage
{
	public CommunitiesPage(CommunitesViewModel viewModel, IServiceProvider serviceProvider)
	{
		BindingContext = viewModel;
		InitializeComponent();
		var nav = serviceProvider.GetRequiredService<NavigationView>();
		Grid.SetRow(nav, 1);
		MyGrid.Children.Add(nav);
	}

}