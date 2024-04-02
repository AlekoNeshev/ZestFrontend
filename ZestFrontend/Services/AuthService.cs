using Auth0.OidcClient;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services;

public class AuthService
{
	public string Id { get; set; }
	public string Username { get; set; }
	public string Token { get; set; }
    public bool IsAdmin { get; set; }
    public List<string> Groups { get; set; } = new List<string>();


	private readonly Auth0Client _client;

	private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

	public AuthService()
	{
		_client = new Auth0Client(new Auth0ClientOptions
		{
			Domain = "dev-kckk4xk2mvwnhizd.us.auth0.com",
			ClientId = "dLljAQ9j9n5Wws5WYJtj8Ne6X4WcQfjc",
			Scope = "openid profile email username",
			RedirectUri = "zest://callback",
			PostLogoutRedirectUri = "zest://callback",
		});
	}

	public async Task<LoginResult> LoginAsync(Dictionary<string, string> extraParameters)
	{
		await _semaphore.WaitAsync();
		try
		{
			return await _client.LoginAsync(extraParameters);
		}
		catch (Exception ex)
		{
			return null;
		}
		finally
		{
			_semaphore.Release();
		}
	}
	public async Task<ClaimsPrincipal> GetAuthenticatedUser()
	{
		ClaimsPrincipal user = null;
		var idToken = await SecureStorage.Default.GetAsync("id_token");

		if (idToken != null)
		{
			var doc = await new HttpClient().GetDiscoveryDocumentAsync($"https://dev-kckk4xk2mvwnhizd.us.auth0.com");
			var validator = new JwtHandlerIdentityTokenValidator();
			var options = new OidcClientOptions
			{
				ClientId ="dLljAQ9j9n5Wws5WYJtj8Ne6X4WcQfjc",
				ProviderInformation = new ProviderInformation
				{
					IssuerName = doc.Issuer,
					KeySet = doc.KeySet
				}
			};
			try
			{
				var validationResult = await validator.ValidateAsync(idToken, options);

				if (!validationResult.IsError) user = validationResult.User;
			}
			catch
			{

			}
		
		}

		return user;
	}
	public async Task LogoutAsync()
	{
		await _semaphore.WaitAsync();
		try
		{
			await _client.LogoutAsync();
		}
		finally
		{
			_semaphore.Release();
		}
	}

}
