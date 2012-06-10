using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FlickrNet;
using Android.Webkit;

namespace Norgegram.Droid
{
	[Activity (Label = "AuthenticationActivity")]			
	public class AuthenticationActivity : Activity
	{
		private const string callbackUrl = "http://localhost/dummy";
		private static OAuthRequestToken requestToken = null;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here			
			SetContentView (Resource.Layout.Authentication);
			
			var webView = FindViewById<WebView> (Resource.Id.webView1);
			webView.Settings.JavaScriptEnabled = true;
			webView.SetWebViewClient (new MyWebViewClient (this));
			
			
			var flickr = FlickrManager.GetInstance (this);
			
			flickr.OAuthGetRequestTokenAsync (callbackUrl, r => {
			
				RunOnUiThread (() => {
				
					if (r.Error != null) {
						Console.WriteLine ("Error " + r.Error.Message);
						SetResult (Result.Canceled);
						Finish ();
						return;
					}
					
					requestToken = r.Result;
					string url = flickr.OAuthCalculateAuthorizationUrl (requestToken.Token, AuthLevel.Write);
					url = url.Replace ("www.flickr.com", "m.flickr.com");
					webView.LoadUrl (url);
					
				});
			
			});
		}

		class MyWebViewClient : WebViewClient
		{
			private readonly AuthenticationActivity _activity;
			public MyWebViewClient (AuthenticationActivity authenticationActivity)
			{
				_activity = authenticationActivity;
			}
			
			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				if (!url.StartsWith (callbackUrl)) {
					return false;
				}
				
				var oauthVerifier = new Uri (url).Query.Split ('&')
					.Where (s => s.Split ('=') [0] == "oauth_verifier")
					.Select (s => s.Split ('=') [1])
					.FirstOrDefault ();
				
				if (string.IsNullOrEmpty (oauthVerifier)) {
					Console.WriteLine ("Unable to find Verifier code in uri: " + url);
					_activity.SetResult (Result.Canceled);
					_activity.Finish ();
				}
				
				var flickr = FlickrManager.GetInstance (_activity);
				flickr.OAuthGetAccessTokenAsync (requestToken, oauthVerifier, result => {
					if (result.Error != null) {
						Console.WriteLine ("An error occurred getting access token: " + result.Error.Message);
						return;
					}
					
					OAuthAccessToken accessToken = result.Result;
					
					FlickrManager.OAuthToken = accessToken.Token;
					FlickrManager.OAuthTokenSecret = accessToken.TokenSecret;
					
					_activity.RunOnUiThread (() => {
						var intent = new Intent ();
						_activity.SetResult (Result.Ok, intent);
						_activity.Finish ();
					});
				});
				
				return true;
			}
		}
	}
}

