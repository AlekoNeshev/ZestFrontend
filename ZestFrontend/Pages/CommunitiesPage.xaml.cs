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
	private void ImageButton_Clicked(object sender, EventArgs e)
	{
		if (SecondImageButton.Rotation == 0)
		{

			SecondImageButton.RotateTo(180);
		}
		else
		{
			SecondImageButton.RotateTo(0);
		}
	}

}