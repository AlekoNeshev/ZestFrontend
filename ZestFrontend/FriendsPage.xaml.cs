using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class FriendsPage : ContentPage
{
	public FriendsPage(FriendsViewModel friendsViewModel)
	{
		BindingContext = friendsViewModel;
		InitializeComponent();
	}
}