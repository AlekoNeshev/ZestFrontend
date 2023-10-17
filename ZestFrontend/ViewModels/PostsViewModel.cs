using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class PostsViewModel : ObservableObject
    {
        PostsService postsService;
      
        public PostsViewModel(PostsService postsService) 
        {
            this.postsService = postsService;
            GetPosts();
        }
        public ObservableCollection<PostDTO> Posts { get; } = new();

        public async void GetPosts()
        {
          

            foreach (var post in await postsService.GetPosts())
            {
                Posts.Add(post);
            }
        }

    }
}
