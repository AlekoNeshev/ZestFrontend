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
using System.Windows.Input;
using ZestFrontend.DTOs;
using ZestFrontend.Parameters;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Community), "Community")]
	public partial class AddPostViewModel : ObservableObject
	{
		PostsService _postsService;
		List<FileResult> _fileResult10;
		MediaService _mediaService;
		
		public AddPostViewModel(PostsService postsService , MediaService mediaService)
        {
            this._postsService = postsService;
			this._mediaService = mediaService;
			_fileResult10 = new List<FileResult>();
			
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
			var isSuccesfull = false;
			var response = await _postsService.AddPost(Title, Content, Community.Id);
			var content = await response.Content.ReadAsStringAsync();
			isSuccesfull = response.IsSuccessStatusCode;
			if(_fileResult10.Count > 0)
			{
				var imageResponse = await _mediaService.UploadImage(int.Parse(content), _fileResult10.ToArray());
				isSuccesfull = imageResponse.IsSuccessStatusCode;
			}
			
			if (isSuccesfull) 
			{
				await Shell.Current.GoToAsync("..");
			}
		}
		[RelayCommand]
		 async Task SelectVideoClicked()
		{
			Images.Clear();
			_fileResult10.Clear();
			var pickOptions = new PickOptions
			{
				PickerTitle = "Select videos",
				FileTypes = FilePickerFileType.Videos
			};
			var fileResult1 = await FilePicker.PickAsync(pickOptions);
			if (fileResult1 != null)
			{
				fileResult1.ContentType = MimeTypesMap.GetMimeType(fileResult1.FileName);
				_fileResult10 .Add(new FileResult(fileResult1));
				Images.Add(fileResult1.FileName);
			}
		}
		[RelayCommand]
		async Task SelectImageClicked()
		{
			Images.Clear();
			_fileResult10.Clear();
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
					_fileResult10.Add (new FileResult(fileResult));
					
					Images.Add(fileResult.FileName);
				}
			}
		}
	}
}
