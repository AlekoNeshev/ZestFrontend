using Auth0.OidcClient;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    
      public partial class LoginViewModel : ObservableObject
    {
        LoginService loginService;
        AuthService authService;
        AccountService accountService;
        HttpClient httpClient;
		public LoginViewModel(LoginService service,  AccountService accountService, HttpClient httpClient) 
        {
            this.loginService = service;
            this.authService = AuthService.Instance;
       this.httpClient = httpClient;
            this.accountService = accountService;
        }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;
         [RelayCommand]
        async Task Login()
        {

            var extraParameters = new Dictionary<string, string>();
			var audience = "https://localhost:7183";

            if (!string.IsNullOrEmpty(audience))
                extraParameters.Add("audience", audience);

            var result = await authService.Client.LoginAsync(extraParameters);
            authService.Token = result.AccessToken;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(authService.Token);
			var usernameClaim = token.Claims.FirstOrDefault(c => c.Type == "username");
			var username = usernameClaim?.Value;

			var account = await accountService.GetCurrentAccount(result.AccessToken);

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authService.Token);
			var response = await httpClient.GetAsync("https://dev-kckk4xk2mvwnhizd.us.auth0.com/userinfo");
			if (response.IsSuccessStatusCode)
			{
				var userProfile = await response.Content.ReadAsStringAsync();
				// parse the userProfile JSON string to extract the user's information
			}

			if (result.IsError == false && account == null)
            {
               
                var user = result.User;
                var name = username;
                var email = user.FindFirst(c => c.Type == "email")?.Value;
                var info = await accountService.CreateAccount(authService.Token, name, email);
                authService.Id = info[0];
                authService.Username = info[1];
            }
            else 
            {
				authService.Id = account.Id;
				authService.Username = account.Username;
			}
           

			await Shell.Current.GoToAsync($"{nameof(PostsPage)}");
			
		}
    }
}
