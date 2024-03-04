using Android.Accounts;
using Android.App;
using Android.Content;
using Android.Content.PM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Platforms.Android
{
	[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
	[IntentFilter(new [] { Intent.ActionView },
			  Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
			  DataScheme = CALLBACK_SCHEME)]
	public class AccountAuthenticatorActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
	{
		const string CALLBACK_SCHEME = "zest";
	}
}
