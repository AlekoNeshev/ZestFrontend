using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZestFrontend.DTOs;
using ZestFrontend.Services;


namespace ZestFrontend.ViewModels
{
    public partial class PostsViewModel : ObservableObject
    {
        PostsService postsService;
        HubConnection connection;
        LikesService likesService;
        AuthService authService;
        public PostsViewModel(PostsService postsService, LikesService service, AuthService authService)
        {

            this.postsService = postsService;
            this.likesService = service;
            this.authService=authService;
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7183/likeshub").Build();
            connection.On<int>("SignalLike", (id)=> UpdatePost(id));
            connection.StartAsync();
            GetPosts();
           
        }
        [ObservableProperty]
        string search;
        
        public ObservableCollection<PostDTO> Posts { get; } = new();
        public async void UpdatePost(int id)
        {
            var updatedPost = await postsService.GetSinglePost(id);
            var post = Posts.Where(x => x.Id==id).First();
            post.Likes = updatedPost.Likes;
            post.Dislikes = updatedPost.Dislikes;
        }
        public async void GetPosts()
        {
            Posts.Clear();
            foreach (var post in await postsService.GetPosts())
            {
                Posts.Add(post);
            }
        }
        [RelayCommand]
         async Task DislikePostAsync(PostDTO postDTO)
        {
           await likesService.Like(authService.Id, postDTO.Id, 0, false);
        }
        [RelayCommand]
        async Task LikePostAsync(PostDTO postDTO)
        {
            await likesService.Like(authService.Id, postDTO.Id, 0, true);
        }
        [RelayCommand]
        async Task GoToPostDetailPageAsync(PostDTO post)
        {
            if (post == null) return;

            await Shell.Current.GoToAsync($"{nameof(PostDetailsPage)}?id={post.Id}", true,
                new Dictionary<string, object>
            {
            {"Post", post }
            });
        }
        [RelayCommand]
        async Task SearchPosts()
        {
            Posts.Clear();
            foreach (var item in await postsService.GetPostsBySearch(Search))
            {
                Posts.Add(item);
            }
        }
    }
}
