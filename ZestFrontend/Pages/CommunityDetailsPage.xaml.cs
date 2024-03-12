using CommunityToolkit.Maui.Markup;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class CommunityDetailsPage : ContentPage
{
	public CommunityDetailsPage(CommunityDetailsViewModel viewModel, IServiceProvider serviceProvider)
	{
		BindingContext = viewModel;
		InitializeComponent();
		var nav = serviceProvider.GetRequiredService<NavigationView>();
		nav.RowSpan(4);
		Grid.SetRow(nav, 1);
		MyGrid.Children.Add(nav);
	}
}