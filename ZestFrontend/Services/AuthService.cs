using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services;

public class AuthService
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
	private static readonly Lazy<AuthService> LazyInstance = new Lazy<AuthService>(() => new AuthService());

	private readonly Auth0Client _client;

	private AuthService()
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

	public static AuthService Instance => LazyInstance.Value;

	public Auth0Client Client => _client;

}
