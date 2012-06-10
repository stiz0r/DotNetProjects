using System;
using System.Linq;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FlickrNet;

namespace Norgegram.iOS
{
	public partial class AuthenticationViewController : UIViewController
	{
		private const  string callbackUrl = "http://localhost/dummy";
		private static OAuthRequestToken requestToken = null;
		
		public event Action<string> AuthenticationCompleted; 
		
		public AuthenticationViewController () : base ("AuthenticationViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			webView.Opaque = false;
			webView.BackgroundColor = UIColor.Clear;
			
			View.BackgroundColor = UIColor.FromPatternImage (new UIImage ("Images/bg-gunmetal.jpg"));
			
			webView.Delegate = new AuthenticationWebViewDelegate (this);
									
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			
			var flickr = FlickrManager.GetInstance ();
			flickr.OAuthGetRequestTokenAsync (callbackUrl, r => {
			
				InvokeOnMainThread (() => {
					if (r.Error != null) {
						Console.WriteLine ("Error " + r.Error.Message);
						return;
					}
					
					requestToken = r.Result;
					
					string url = flickr.OAuthCalculateAuthorizationUrl (requestToken.Token, AuthLevel.Write);
					url = url.Replace ("www.flickr.com", "m.flickr.com");
					webView.LoadRequest (new NSUrlRequest (new NSUrl (url)));
				});
			
			});
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		class AuthenticationWebViewDelegate : UIWebViewDelegate
		{
			private AuthenticationViewController _avc;
			
			public AuthenticationWebViewDelegate (AuthenticationViewController avc)
			{
				_avc = avc;
			}
			
			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{
				if (!request.Url.AbsoluteUrl.ToString ().StartsWith (callbackUrl)) {
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
					return true;
				}
				
				var oauthVerifier = request.Url.Query.Split ('&')
					.Where (s => s.Split ('=') [0] == "oauth_verifier")
					.Select (s => s.Split ('=') [1])
					.FirstOrDefault ();
				
				if (string.IsNullOrEmpty (oauthVerifier)) {
					Console.WriteLine ("Unable to find Verifier code in uri: " + request.Url.AbsoluteUrl);
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
					return true;
				}
				
				var flickr = FlickrManager.GetInstance ();
				flickr.OAuthGetAccessTokenAsync (requestToken, oauthVerifier, r => {
					if (r.Error != null) {
						Console.WriteLine ("An error occurred getting access token: " + r.Error.Message);
						return;
					}
					
					OAuthAccessToken accessToken = r.Result;
					FlickrManager.OAuthToken = accessToken.Token;
					FlickrManager.OAuthTokenSecret = accessToken.TokenSecret;
					
					InvokeOnMainThread (() => {
						_avc.AuthenticationCompleted (accessToken.Token);
					});
				});
				return false;
			}	
			
			public override void LoadingFinished (UIWebView webView)
			{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			}
		}
	}
}

