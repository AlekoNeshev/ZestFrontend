using CommunityToolkit.Mvvm.ComponentModel;
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
	}
}
