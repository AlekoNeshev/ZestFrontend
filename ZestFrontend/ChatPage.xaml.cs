using ZestFrontend.ViewModels;

namespace ZestFrontend;

public partial class ChatPage : ContentPage
{
	public ChatPage(ChatViewModel chatViewModel)
	{
		BindingContext = chatViewModel;
		InitializeComponent();
	}
}