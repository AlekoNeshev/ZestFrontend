
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.DependencyInjection;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend.Pages;

public partial class CommentDetailsPage : ContentPage
{
	public CommentDetailsPage(CommentDetailsViewModel commentDetailsViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = commentDetailsViewModel;
		InitializeComponent();
		if (Device.RuntimePlatform == Device.WinUI)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();
			nav.Padding(0, 64);
			MyGrid.Children.Add(nav);
		}
	}
}