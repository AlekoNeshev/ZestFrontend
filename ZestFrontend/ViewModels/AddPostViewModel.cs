using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyRed.Mime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		FileResult fileResult;
		MediaService mediaService;
		public AddPostViewModel(PostsService postsService , MediaService mediaService)
        {
            this.postsService = postsService;
			this.authService = AuthService.Instance;
			this.mediaService = mediaService;
        }

		[ObservableProperty]
		CommunityDTO community;
		[ObservableProperty]
		string title;
		[ObservableProperty]
		string content;
		[ObservableProperty]
		string imageName;
		
		[RelayCommand]
		async Task CreatePost()
		{
			if(Title == null || Content == null) 
			{
				return;
			}
			var response = await postsService.AddPost(Title, Content, Community.Id);
			var content = await response.Content.ReadAsStringAsync();
			var imageResponse = await mediaService.UploadImage(int.Parse(content), fileResult);
			if (response.IsSuccessStatusCode && imageResponse.IsSuccessStatusCode) 
			{
				await Shell.Current.GoToAsync($"{nameof(CommunityDetailsPage)}?id={Community.Name}", true,
				new Dictionary<string, object>
			{
			{"Community", Community }
			});
			}
		}
		[RelayCommand]
		 async Task SelectImageClicked()
		{
			var fileResult1 = await FilePicker.PickAsync(new PickOptions());
			if (fileResult1 != null)
			{
				//fileResult1.ContentType;
				fileResult = fileResult1;
				fileResult.ContentType = MimeTypesMap.GetMimeType(fileResult1.FileName);
				ImageName = fileResult1.FileName;
			}
		}
	}
}
