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
		List<FileResult> _fileResults;
		MediaService _mediaService;
		
		public AddPostViewModel(PostsService postsService , MediaService mediaService)
        {
            this._postsService = postsService;
			this._mediaService = mediaService;
			_fileResults = new List<FileResult>();
			
		}

		[ObservableProperty]
		CommunityDTO community;
		[ObservableProperty]
		string title;
		[ObservableProperty]
		string content;
		public ObservableCollection<string> Files { get; private set; } = new();

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
			if(_fileResults.Count > 0)
			{
				var imageResponse = await _mediaService.UploadImage(int.Parse(content), _fileResults.ToArray());
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
			Files.Clear();
			_fileResults.Clear();
			var pickOptions = new PickOptions
			{
				PickerTitle = "Select videos",
				FileTypes = FilePickerFileType.Videos
			};
			var pickedFiles = await FilePicker.PickAsync(pickOptions);
			if (pickedFiles != null)
			{
				pickedFiles.ContentType = MimeTypesMap.GetMimeType(pickedFiles.FileName);
				_fileResults .Add(new FileResult(pickedFiles));
				Files.Add(pickedFiles.FileName);
			}
		}
		[RelayCommand]
		async Task SelectImageClicked()
		{
			Files.Clear();
			_fileResults.Clear();
			var pickOptions = new PickOptions
			{
				PickerTitle = "Select images",
				FileTypes = FilePickerFileType.Images
			};

			var pickedFiles = await FilePicker.PickMultipleAsync(pickOptions);
			if (pickedFiles.Count()>5)
			{
				Files.Clear();
				return;
			}
			if (pickedFiles != null && pickedFiles.Count() > 0)
			{			
				foreach (var fileResult in pickedFiles)
				{
					fileResult.ContentType = MimeTypesMap.GetMimeType(fileResult.FileName);
					_fileResults.Add (new FileResult(fileResult));
					
					Files.Add(fileResult.FileName);
				}
			}
		}
	}
}
