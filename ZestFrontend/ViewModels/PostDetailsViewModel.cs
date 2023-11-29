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
    [QueryProperty(nameof(Post), "Post")]
    public partial class PostDetailsViewModel : ObservableObject
    {
         AuthService authService;
         PostsService postsService;
         LikesService likesService;

        public PostDetailsViewModel(AuthService authService, PostsService postsService, LikesService likesService) 
        { 
            this.authService = authService;
            this.postsService = postsService;
            this.likesService = likesService;
        }
        [ObservableProperty]
        PostDTO post;
        public ObservableCollection<CommentDTO> Comments { get; } = new();

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
    }
}
