using Auth0.OidcClient;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
    
      public partial class LoginViewModel : ObservableObject
    {
        LoginService service;
        AuthService authService;
		Auth0Client client = new Auth0Client(new Auth0ClientOptions
		{
			Domain = "dev-kckk4xk2mvwnhizd.us.auth0.com",
			ClientId = "dLljAQ9j9n5Wws5WYJtj8Ne6X4WcQfjc",
			Scope = "openid profile email",
			RedirectUri = "zest://callback",
			PostLogoutRedirectUri = "zest://callback",
		});
		public LoginViewModel(LoginService service, AuthService authService) 
        {
            this.service = service;
            this.authService = authService;
            //this.authenticationService = authenticationService;
        }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;
         [RelayCommand]
        async Task Login()
        {

            var extraParameters = new Dictionary<string, string>();
            var audience = "https://localhost:7183"; // FILL WITH AUDIENCE AS NEEDED

            if (!string.IsNullOrEmpty(audience))
                extraParameters.Add("audience", audience);

            var result = await client.LoginAsync(extraParameters);
            authService.Token = result.AccessToken;
			await Shell.Current.GoToAsync($"{nameof(PostsPage)}");
			/*AccountDTO account = await service.GetAccount(Username, Password);
			if (account == null)
            {
                return;
            }
            else
            {
                authService.Id = account.Id;
                authService.Username = account.Username;
                await Shell.Current.GoToAsync($"{nameof(PostsPage)}");
            }*/
		}
    }
}
