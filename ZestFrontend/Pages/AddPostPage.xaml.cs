using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class AddPostPage : ContentPage
{
	public AddPostPage(AddPostViewModel addPostViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = addPostViewModel;
		InitializeComponent();
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			MyGrid.Children.Add(nav);
		}
	}
}