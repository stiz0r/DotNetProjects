using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Provider;
using System.Threading;
using Xamarin.Geolocation;
using Xamarin.Media;
using System.Threading.Tasks;
using Android.Graphics;
using System.IO;

namespace Norgegram.Droid
{
	[Activity (Label = "Norgegram", MainLauncher = true)]
	public class Activity1 : Activity
	{
		int count = 1;
		int authenticationRequestCode = 100;
		int photoPickerCode = 200;
		Java.IO.File _file;
		
		private Geolocator locationManager;
		private Position location;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
						
			
			UpdateLocation ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			
			var linearLayout = FindViewById<LinearLayout> (Resource.Id.linearLayout);
			linearLayout.SetBackgroundResource (Resource.Drawable.bggunmetal);
			
			
			var captureButton = FindViewById <Button> (Resource.Id.captureButton);
			captureButton.SetBackgroundResource (Resource.Drawable.bluebutton);
			
			captureButton.Click += (sender, e) => {
				
				var flickr = FlickrManager.GetAuthInstance (this);
				if (!flickr.IsAuthenticated) {				
					Intent intent = new Intent (this, typeof(AuthenticationActivity));
					StartActivityForResult (intent, authenticationRequestCode);	
				} else {
					TakePhoto ();
				}
			};
		}
		
		private void TakePhoto ()
		{
			var picker = new MediaPicker (this);
			if (picker.IsCameraAvailable) {
				picker.TakePhotoAsync (new StoreCameraMediaOptions ())
					.ContinueWith (r => ProcessImage (r));
			} else {
				picker.PickPhotoAsync ().ContinueWith (r => ProcessImage (r));
			}
		}

		private void ProcessImage (Task<MediaFile> task)
		{
			var imageView = FindViewById<ImageView> (Resource.Id.imageView);
			
			RunOnUiThread (() => {
				using (var bitmap = BitmapFactory.DecodeFile (task.Result.Path)) {
					imageView.SetImageBitmap (bitmap);
				}
			});	
				
			UploadImage (task.Result.GetStream ());		
		}	
		
		private void UpdateLocation ()
		{
			locationManager = new Geolocator (this);
			locationManager.PositionChanged += (sender, e) => {
				location = e.Position;
				locationManager.StopListening ();
			};
			locationManager.StartListening (0, 0);
		}
		
		protected override void OnActivityResult (int requestCode, Result resultCode, Android.Content.Intent data)
		{
			if (requestCode == authenticationRequestCode) {
				if (resultCode == Result.Ok) {
					TakePhoto ();
				} else {
					Console.WriteLine ("Activity Canceled");
				}
			}
		}
		
		private void UploadImage (Stream imageStream)
		{
			var flickr = FlickrManager.GetAuthInstance (this);
			flickr.UploadPictureAsync (imageStream, "filename", "NDC2012", "Picture from #MonoForAndroid", "NDCOslo", true, false, false, FlickrNet.ContentType.Photo, FlickrNet.SafetyLevel.Safe, FlickrNet.HiddenFromSearch.Visible, result => {
				imageStream.Dispose ();
				
				RunOnUiThread (() => {
					new AlertDialog.Builder (this)
						.SetMessage ("Uploaded")
						.SetNeutralButton ("OK!", (o, e) => {})
						.Show ();
				});
				
				if (location != null) {
					flickr.PhotosGeoSetLocationAsync (result.Result, location.Latitude, location.Longitude, a => {
						Console.WriteLine ("Uplaoded the image location too...");
					});
				}
			});
		}
	}
}