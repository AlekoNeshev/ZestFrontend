
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.ViewModels
{

	public partial class MainViewModel : ObservableObject
	{

		public MainViewModel() { }

		[RelayCommand]
		async Task GoToLogin()
		{
			await Shell.Current.GoToAsync(nameof(Login));
		}
		[RelayCommand]
		async Task GoToRegisterAsync()
		{
			
		}
	}
}
