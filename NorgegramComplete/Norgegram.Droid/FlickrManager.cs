using System;
using FlickrNet;
using Android.App;
using Android.Preferences;

namespace Norgegram.Droid
{
	public class FlickrManager
	{
		public const string ApiKey = "9edc75f110259cb7f27a4ea4ee8d2e76";
		public const string SharedSecret = "ac77454e0d5966d7";
		
		private static Activity _activity;

		public static Flickr GetInstance (Activity activity)
		{
			_activity = activity;			
			return new Flickr (ApiKey, SharedSecret);
		}

		public static Flickr GetAuthInstance (Activity activity)
		{
			_activity = activity;			
			var f = new Flickr (ApiKey, SharedSecret);
			f.OAuthAccessToken = OAuthToken;
			f.OAuthAccessTokenSecret = OAuthTokenSecret;
			return f;
		}
		
		public static string OAuthToken {
			get {
				return GetValue ("OAuthToken");
			}
			set {
				SetValue ("OAuthToken", value);
			}
		}

		public static string OAuthTokenSecret {
			get {
				return GetValue ("OAuthTokenSecret");
			}
			set {
				SetValue ("OAuthTokenSecret", value);
			}
		}

		private static string GetValue (string key)
		{
			return PreferenceManager.GetDefaultSharedPreferences (_activity).GetString (key, null);
		}
		
		private static void SetValue (string key, string value)
		{
			PreferenceManager.GetDefaultSharedPreferences (_activity)
				.Edit ()
				.PutString (key, value)
				.Commit ();
		}
	}
}