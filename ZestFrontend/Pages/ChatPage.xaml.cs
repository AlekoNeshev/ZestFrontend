
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using System;
using ZestFrontend.DTOs;
using ZestFrontend.ViewModels;
using ZestFrontend.CustomViews;

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
		if (Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.WinUI || Microsoft.Maui.Devices.DeviceInfo.Current.Platform == Microsoft.Maui.Devices.DevicePlatform.macOS)
		{
			var nav = serviceProvider.GetRequiredService<NavigationView>();

			nav.Paddings(0, 64, 0, 0);

			MyGrid.Children.Add(nav);
		}
		Messages.Scrolled += Messages_Scrolled;
		viewModel.NewMessageReceived += ViewModel_NewMessageReceived;
		viewModel.OnOpenScreen +=  ViewModel_OnOpenScreen;
	}
	private async void Messages_Scrolled(object sender, ItemsViewScrolledEventArgs e)
	{
		const int tolerance = 1;

		if (e.LastVisibleItemIndex >= viewModel.Messages.Select(x => x.Count).Sum() - 1 - tolerance)
		{
			_isUserAtEndOfCollection = true;
		}
		else if (e.FirstVisibleItemIndex == 0)
		{

			if (viewModel != null && viewModel.IsLoadingMessages == false)
			{
				Messages.Scale.ToString();
				await viewModel.LoadMoreMessages();
			}
			_isUserAtEndOfCollection = false;
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
		int lastGroupIndex = viewModel.Messages.Count - 1;
		var lastItemIndex = viewModel.Messages.LastOrDefault().LastOrDefault();

		if (lastGroupIndex >= 0 && lastItemIndex != null)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				if (_isUserAtEndOfCollection)
				{
					Messages.ScrollTo(lastItemIndex, position: ScrollToPosition.MakeVisible, animate: false);
				}
			});
		}
	}
	private void ViewModel_OnOpenScreen(object sender, EventArgs e)
	{
		int lastGroupIndex = viewModel.Messages.Count - 1;
		if (lastGroupIndex >= 0)
		{
			var lastItemIndex = viewModel.Messages.LastOrDefault().LastOrDefault();

			if (lastItemIndex != null)
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					Messages.ScrollTo(lastItemIndex, position: ScrollToPosition.MakeVisible, animate: false);
				});
			}
		}
	
		
	}
	private void Button_Clicked(object sender, EventArgs e)
	{
		CommentEntry.Text = string.Empty;
	}
}