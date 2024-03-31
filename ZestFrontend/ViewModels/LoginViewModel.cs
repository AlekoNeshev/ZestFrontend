using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.OidcClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{

	public partial class LoginViewModel : ObservableObject
    {
        LoginService _loginService;
        AuthService _authService;
        AccountService _accountService;
        HttpClient _httpClient;
		public LoginViewModel(LoginService service,  AccountService accountService, HttpClient httpClient, AuthService authService) 
        {
            this._loginService = service;
            this._authService = authService;
       this._httpClient = httpClient;
            this._accountService = accountService;
            Retrieve();
        }
        public async void Retrieve()
        {
			var userId = await _authService.GetAuthenticatedUser();
            if(userId != null)
            {
				var accessToken = await SecureStorage.Default.GetAsync("access_token");
				var account = await _accountService.GetCurrentAccount(accessToken);
				_authService.Token = accessToken;
				_authService.Id = account.Id;
				_authService.Username = account.Username;
				_authService.IsAdmin = account.IsAdmin;
				await Shell.Current.GoToAsync($"{nameof(PostsPage)}");
			}
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

			var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(_authService.Token);
			var usernameClaim = token.Claims.FirstOrDefault(c => c.Type == "username");
			
			var account = await _accountService.GetCurrentAccount(result.AccessToken);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
			var response = await _httpClient.GetAsync("https://dev-kckk4xk2mvwnhizd.us.auth0.com/userinfo");
			if (response.IsSuccessStatusCode)
			{
				var userProfile = await response.Content.ReadAsStringAsync();
				// parse the userProfile JSON string to extract the user's information
			}

			if (result.IsError == false && account == null)
            {
               
                var user = result.User;
                var name = account.Username;
                var email = user.FindFirst(c => c.Type == "email")?.Value;
                var info = await _accountService.CreateAccount(_authService.Token, name, email);
                _authService.Id = info[0];
                _authService.Username = info[1];
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
