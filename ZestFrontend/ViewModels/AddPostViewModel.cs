using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Community), "Community")]
	public partial class AddPostViewModel : ObservableObject
	{
		PostsService postsService;
		AuthService authService;
        public AddPostViewModel(PostsService postsService, AuthService authService)
        {
            this.postsService = postsService;
			this.authService = authService;
        }

		[ObservableProperty]
		CommunityDTO community;
		[ObservableProperty]
		string title;
		[ObservableProperty]
		string content;

		[RelayCommand]
		async Task CreatePost()
		{
			if(Title == null || Content == null) 
			{
				return;
			}
			var response = await postsService.AddPost(Title, Content, Community.Id, authService.Id);
			if (response.IsSuccessStatusCode) 
			{
				await Shell.Current.GoToAsync($"{nameof(CommunityDetailsPage)}?id={Community.Name}", true,
				new Dictionary<string, object>
			{
			{"Community", Community }
			});
			}
		}
	}
}
