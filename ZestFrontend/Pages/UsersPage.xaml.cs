using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class UsersPage : ContentPage
{
	UsersViewModel viewModel;
	public UsersPage(UsersViewModel usersViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = usersViewModel;
		this.viewModel = usersViewModel;
		InitializeComponent();
		var nav = serviceProvider.GetRequiredService<NavigationView>();
		Grid.SetRow(nav, 1);
		MyGrid.Children.Add(nav);
	}
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		viewModel.OnNavaigated();
		base.OnNavigatedTo(args);
	}
}