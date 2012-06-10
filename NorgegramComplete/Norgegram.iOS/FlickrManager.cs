using System;
using FlickrNet;
using MonoTouch.Foundation;

namespace Norgegram
{
	public class FlickrManager
	{
		public const string ApiKey = "9edc75f110259cb7f27a4ea4ee8d2e76";
		public const string SharedSecret = "ac77454e0d5966d7";

		public static Flickr GetInstance ()
		{
			return new Flickr (ApiKey, SharedSecret);
		}

		public static Flickr GetAuthInstance ()
		{
			var f = new Flickr (ApiKey, SharedSecret);
			f.OAuthAccessToken = OAuthToken;
			f.OAuthAccessTokenSecret = OAuthTokenSecret;
			return f;
		}
		
		public static string OAuthToken {
			get {
				if (HasValue ("OAuthToken"))
					return GetValue ("OAuthToken");
				else {
					return null;
				}
			}
			set {
				SetValue ("OAuthToken", value);	
			}
		}

		public static string OAuthTokenSecret {
			get {
				if (HasValue ("OAuthTokenSecret"))
					return GetValue ("OAuthTokenSecret");
				else {
					return null;
				}
			}
			set {
				SetValue ("OAuthTokenSecret", value);
			}
		}
				
		private static bool HasValue (string key)
		{
			return !string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(key));
		}
		
		private static string GetValue (string key)
		{
			return NSUserDefaults.StandardUserDefaults.StringForKey (key);
		}
		
		private static void SetValue (string key, string value)
		{
			NSUserDefaults.StandardUserDefaults.SetString (value, key);
			NSUserDefaults.StandardUserDefaults.Synchronize ();
		}			
	}
}