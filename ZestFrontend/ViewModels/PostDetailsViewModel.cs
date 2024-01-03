using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
        HubConnection likesConnection;
        CommentService commentService;

        public PostDetailsViewModel(AuthService authService, PostsService postsService, LikesService likesService, CommentService commentService) 
        { 
            this.authService = authService;
            this.postsService = postsService;
            this.likesService = likesService;          
            this.commentService = commentService;
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7183/commentshub").Build();
            likesConnection = new HubConnectionBuilder().WithUrl("https://localhost:7183/likeshub").Build();
            connection.On("CommentPosted", GetComments);
            likesConnection.On<int>("CommentLiked", UpdateComment);
            connection.StartAsync();
            likesConnection.StartAsync();

        }
        [ObservableProperty]
        PostDTO post;
        public ObservableCollection<CommentDTO> Comments { get; private set; } = new();
        [ObservableProperty]
        bool isBusy;

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
        
        public async void GetComments()
        {
            Comments.Clear();
            var comments = await commentService.GetComments(Post.Id);
            foreach (var comment in comments)
            {
                Comments.Add(comment);
            }
        }
        [RelayCommand]
        async Task SendAsync(string text)
        {
            await commentService.PostComment(Post.Id, authService.Id, text);
        }
        partial void OnPostChanged(PostDTO value)
        {
            GetComments();
        }
        [RelayCommand]
        async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync(nameof(PostsPage));
        }
        [RelayCommand]
        async Task LikeCommentAsync(CommentDTO commentDTO)
        {
           await likesService.Like(authService.Id, 0, commentDTO.Id, true);
        }
        [RelayCommand]
        async Task DislikeCommentAsync(CommentDTO commentDTO)
        {
            await likesService.Like(authService.Id, 0, commentDTO.Id, false);
        }
        public async void UpdateComment(int id)
        {
            var updatedComment = await commentService.GetSingleComment(id);
            var comment = Comments.Where(x => x.Id==id).First();
            comment.Likes = updatedComment.Likes;
            comment.Dislikes = updatedComment.Dislikes;
        }
        [RelayCommand]
        async Task DeleteCommentAsync(CommentDTO commentDTO)
        {
            await commentService.DeleteComment(commentDTO.Id);
        }
    }
}
