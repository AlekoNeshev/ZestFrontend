using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class UsersPage : ContentPage
{
	UsersViewModel viewModel;
	public UsersPage(UsersViewModel usersViewModel)
	{
		BindingContext = usersViewModel;
		this.viewModel = usersViewModel;
		InitializeComponent();
	}
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		viewModel.OnNavaigated();
		base.OnNavigatedTo(args);
	}
}