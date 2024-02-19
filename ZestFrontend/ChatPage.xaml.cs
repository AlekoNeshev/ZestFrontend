using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class ChatPage : ContentPage
{
	ChatViewModel viewModel;
	public ChatPage(ChatViewModel chatViewModel)
	{
		BindingContext = chatViewModel;
		this.viewModel = chatViewModel;
		InitializeComponent();
	}
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		viewModel.OnNavigatedTo();
		base.OnNavigatedTo(args);
	}
}