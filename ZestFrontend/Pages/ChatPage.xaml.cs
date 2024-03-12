
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using ZestFrontend.DTOs;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend;

public partial class ChatPage : ContentPage
{
	ChatViewModel viewModel;
	private bool _isUserAtEndOfCollection;
	public ChatPage(ChatViewModel chatViewModel, IServiceProvider serviceProvider)
	{
		BindingContext = chatViewModel;
		this.viewModel = chatViewModel;
		InitializeComponent();
		var nav = serviceProvider.GetRequiredService<NavigationView>();
		viewModel.NewMessageReceived += ViewModel_NewMessageReceived;
		MyGrid.Children.Add(nav);
		Messages.Scrolled += Messages_Scrolled;
	}
	private void Messages_Scrolled(object sender, ItemsViewScrolledEventArgs e)
	{
		// Check if the user is already at the end of the collection (within a small tolerance)
		const int tolerance = 1; // Adjust the tolerance as needed
		if (e.LastVisibleItemIndex >= viewModel.Messages.Count - 1 - tolerance)
		{
			_isUserAtEndOfCollection = true;
		}
		else
		{
			_isUserAtEndOfCollection = false;
		}
	}
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		viewModel.OnNavigatedTo();
		base.OnNavigatedTo(args);
	}
	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		viewModel.OnNavigatedFrom();
		base.OnNavigatedFrom(args);
	}

	private void ViewModel_NewMessageReceived(object sender, EventArgs e)
	{
		// Access the ScrollView and scroll to the end

		int lastIndex = viewModel.Messages.Count - 1;

		if (lastIndex >= 0)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				if (_isUserAtEndOfCollection)
				{
					Messages.ScrollTo(lastIndex, position: ScrollToPosition.MakeVisible, animate: false);
				}
				

				
				
			});
		}
		

	}
	/*private void Messages_ChildAdded(object sender, ElementEventArgs e)
	{
		try
		{

			ChatViewModel viewModel = BindingContext as ChatViewModel;
			MessageDTO monkey = viewModel.Messages.LastOrDefault();
			Messages.ScrollTo(monkey);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}*/
}