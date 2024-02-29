/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Auth0.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient;


namespace ZestFrontend.Services
{
	public class AuthenticationService
	{
		

		private async void OnLoginClicked(object sender, EventArgs e)
		{
			var extraParameters = new Dictionary<string, string>();
			var audience = ""; // FILL WITH AUDIENCE AS NEEDED

			if (!string.IsNullOrEmpty(audience))
				extraParameters.Add("audience", audience);

			var result = await client.LoginAsync(extraParameters);

			DisplayResult(result);
		}

		private async void OnLogoutClicked(object sender, EventArgs e)
		{
			BrowserResultType browserResult = await client.LogoutAsync();

			if (browserResult != BrowserResultType.Success)
			{

				return;
			}


		}

		private void DisplayResult(LoginResult loginResult)
		{
			if (loginResult.IsError)
			{

				return;
			}


		}
	}
}
*/