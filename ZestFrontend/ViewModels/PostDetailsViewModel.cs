using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
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
    [QueryProperty(nameof(Post), "Post")]
    public partial class PostDetailsViewModel : ObservableObject
    {
         AuthService authService;
         PostsService postsService;
         LikesService likesService;
         HubConnection connection;

        public PostDetailsViewModel(AuthService authService, PostsService postsService, LikesService likesService) 
        { 
            this.authService = authService;
            this.postsService = postsService;
            this.likesService = likesService;
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7183/likeshub").Build();
            connection.On<int>("SignalLike", (id) => UpdatePost(id));
            connection.StartAsync();
        }
        [ObservableProperty]
        PostDTO post;
        public ObservableCollection<CommentDTO> Comments { get; } = new();

        public async void UpdatePost(int id)
        {
            var updatedPost = await postsService.GetSinglePost(id);
           /* Post.Likes = updatedPost.Likes;
            Post.Dislikes = updatedPost.Dislikes;*/
        }

        [RelayCommand]
        async Task DislikePostAsync()
        {
            await likesService.Like(authService.Id, Post.Id, 0, false);
        }
        [RelayCommand]
        async Task LikePostAsync()
        {
            await likesService.Like(authService.Id, Post.Id, 0, true);
        }
        [RelayCommand]
        async Task SendAsync()
        {
           // await likesService.Like(authService.Id, postDTO.Id, 0, true);
        }
    }
}
