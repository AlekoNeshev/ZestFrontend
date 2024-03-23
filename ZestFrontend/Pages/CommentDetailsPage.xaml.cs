
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
		var nav = serviceProvider.GetRequiredService<NavigationView>();

		
		MyGrid.Children.Add(nav);
	}
}