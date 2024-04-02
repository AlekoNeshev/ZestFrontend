
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{

	public partial class MainViewModel : ObservableObject
	{
		LoginService _loginService;
		AuthService _authService;
		AccountService _accountService;
		public MainViewModel(LoginService service, AccountService accountService, AuthService authService) 
		{
			this._loginService = service;
			this._authService = authService;
			this._accountService = accountService;
			Retrieve();
		}		
		public async void Retrieve()
		{
			IsSearching = true;
			IsNotSearching = false;
			var userId = await _authService.GetAuthenticatedUser();
			if (userId != null)
			{
				var accessToken = await SecureStorage.Default.GetAsync("access_token");
				var account = await _accountService.GetCurrentAccount(accessToken);
				_authService.Token = accessToken;
				_authService.Id = account.Id;
				_authService.Username = account.Username;
				_authService.IsAdmin = account.IsAdmin;
				await Shell.Current.GoToAsync($"{nameof(PostsPage)}");
			}
			IsSearching = false;
			IsNotSearching = true;
		}

		[ObservableProperty]
		string username;
		[ObservableProperty]
		string password;
		[ObservableProperty]
		bool isSearching;
		[ObservableProperty]
		bool isNotSearching;

		[RelayCommand]
		async Task Login()
		{

			var extraParameters = new Dictionary<string, string>();
			var audience = "https://localhost:7183";

			if (!string.IsNullOrEmpty(audience))
				extraParameters.Add("audience", audience);

			var result = await _authService.LoginAsync(extraParameters);
			_authService.Token = result.AccessToken;
			try
			{
				await SecureStorage.Default.SetAsync("access_token", result.AccessToken);
				await SecureStorage.Default.SetAsync("id_token", result.IdentityToken);
			}
			catch (Exception ex)
			{

			}

			var account = await _accountService.GetCurrentAccount(result.AccessToken);


			if (result.IsError == false && account == null)
			{
				var handler = new JwtSecurityTokenHandler();
				var token = handler.ReadJwtToken(_authService.Token);
				var usernameClaim = token.Claims.FirstOrDefault(c => c.Type == "username");
				var user = result.User;
				var name = usernameClaim.Value;
				var email = user.FindFirst(c => c.Type == "email")?.Value;
				var info = await _accountService.CreateAccount(_authService.Token, name, email);
				_authService.Id = info.Id;
				_authService.Username = info.Username;
			}
			else
			{
				_authService.Id = account.Id;
				_authService.Username = account.Username;
				_authService.IsAdmin = account.IsAdmin;
			}

			await Shell.Current.GoToAsync($"{nameof(PostsPage)}");

		}
	}
}
