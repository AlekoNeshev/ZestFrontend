using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    [QueryProperty(nameof(Community), "Community")]
    public partial class CommunityDetailsViewModel : ObservableObject
    {
        CommunityService communityService;
        PostsService postsService;
        public CommunityDetailsViewModel(CommunityService communityService, PostsService postsService)
        {
            this.communityService = communityService;
            this.postsService = postsService;
        }

        [ObservableProperty]
        CommunityDTO community;
		public ObservableCollection<PostDTO> Posts { get; private set; } = new();

		[RelayCommand]
        async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync(nameof(CommunitiesPage));
        }
		public async void GetComments()
		{
			Posts.Clear();
			var posts = await postsService.GetPostsByCommunity(Community.Id);
			foreach (var post in posts)
			{
				Posts.Add(post);
			}
		}
		partial void OnCommunityChanged(CommunityDTO value)
		{
			GetComments();
		}
	}
}
