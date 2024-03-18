using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyRed.Mime;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		List<FileResult> fileResult10;
		MediaService mediaService;
		public AddPostViewModel(PostsService postsService , MediaService mediaService, AuthService authService)
        {
            this.postsService = postsService;
			this.authService = authService;
			this.mediaService = mediaService;
			fileResult10 = new List<FileResult>();
        }

		[ObservableProperty]
		CommunityDTO community;
		[ObservableProperty]
		string title;
		[ObservableProperty]
		string content;
		public ObservableCollection<string> Images { get; private set; } = new();

		[RelayCommand]
		async Task CreatePost()
		{
			if(Title == null || Content == null) 
			{
				return;
			}
			var response = await postsService.AddPost(Title, Content, Community.Id);
			var content = await response.Content.ReadAsStringAsync();
			var imageResponse = await mediaService.UploadImage(int.Parse(content), fileResult10.ToArray());
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
		 async Task SelectVideoClicked()
		{
			Images.Clear();
			fileResult10.Clear();
			var pickOptions = new PickOptions
			{
				PickerTitle = "Select videos",
				FileTypes = FilePickerFileType.Videos
			};
			var fileResult1 = await FilePicker.PickAsync(pickOptions);
			if (fileResult1 != null)
			{
				fileResult1.ContentType = MimeTypesMap.GetMimeType(fileResult1.FileName);
				fileResult10 .Add(new FileResult(fileResult1));
			
				/*fileResult.
				fileResult = fileResult1;
				fileResult.ContentType = MimeTypesMap.GetMimeType(fileResult1.FileName);*/
				Images.Add(fileResult1.FileName);
			}
		}
		[RelayCommand]
		async Task SelectImageClicked()
		{
			Images.Clear();
			fileResult10.Clear();
			var pickOptions = new PickOptions
			{
				PickerTitle = "Select images",
				FileTypes = FilePickerFileType.Images
			};

			var fileResults = await FilePicker.PickMultipleAsync(pickOptions);
			if (fileResults != null && fileResults.Count() > 0)
			{
				
				
				foreach (var fileResult in fileResults)
				{
					fileResult.ContentType = MimeTypesMap.GetMimeType(fileResult.FileName);
					fileResult10.Add (new FileResult(fileResult));
					
					Images.Add(fileResult.FileName);
				}
			}
		}
	}
}
