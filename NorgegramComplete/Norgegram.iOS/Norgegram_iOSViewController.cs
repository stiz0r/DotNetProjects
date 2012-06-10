using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.ObjCRuntime;
using System.Threading;
using System.IO;
using Xamarin.Geolocation;
using Xamarin.Media;
using System.Threading.Tasks;

namespace Norgegram.iOS
{
	public partial class Norgegram_iOSViewController : UIViewController
	{
		private Geolocator locationManager;
		private Position location;
		
		public Norgegram_iOSViewController () : base ("Norgegram_iOSViewController", null)
		{
				
		}
		
		[Export("UpdateLocation")]
		private void UpdateLocation ()
		{
			locationManager = new Geolocator ();
			locationManager.PositionChanged += (sender, e) => {
				location = e.Position;
				locationManager.StopListening ();
			};
			locationManager.StartListening (0, 0);
		}
		
		[Export("TakeAPhoto")]
		private void TakeAPhoto ()
		{
			if (this.ModalViewController != null) {
				this.PerformSelector (new Selector ("TakeAPhoto"), null, 1.0f);
				return;
			}
			
			var picker = new MediaPicker ();
			if (picker.IsCameraAvailable) {
				picker.TakePhotoAsync (new StoreCameraMediaOptions ())
					.ContinueWith (t => ProcessImage (t));
			} else {
				picker.PickPhotoAsync ().ContinueWith (t => ProcessImage (t));
			}
			
			/*
			imagePicker = new UIImagePickerController ();
			imagePicker.SourceType = 
				UIImagePickerController.IsSourceTypeAvailable (UIImagePickerControllerSourceType.Camera) ?
					UIImagePickerControllerSourceType.Camera :
					UIImagePickerControllerSourceType.PhotoLibrary;
			
			imagePicker.FinishedPickingMedia += (sender, e) => {
				imageView.Image = e.OriginalImage;
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
				ThreadPool.QueueUserWorkItem (callback => {
					UploadImage (e.OriginalImage);
				});
				this.DismissModalViewControllerAnimated (true);
			};
			
			imagePicker.Canceled += (sender, e) => {
				this.DismissModalViewControllerAnimated (true);
			};
			
			this.PresentModalViewController (imagePicker, true);*/
		}

		private void ProcessImage (Task<MediaFile> t)
		{
			InvokeOnMainThread (() => {
				imageView.Image = UIImage.FromFile (t.Result.Path);
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			});
			UploadImage (t.Result.GetStream());			
		}	
		
		private void UploadImage (Stream s)
		{
			var flickr = FlickrManager.GetAuthInstance ();					
			flickr.UploadPictureAsync (s, "filename", "Norgegram #ndcoslo", "Taken with awesome MonoTouch app", "ndcoslo", true, false, false, FlickrNet.ContentType.Photo, FlickrNet.SafetyLevel.Safe, FlickrNet.HiddenFromSearch.None, result => { 
				InvokeOnMainThread (() => {
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					using (var alert = new UIAlertView("W00t!", "Uploaded your image!", null, "OK", null)) {
						alert.Show ();
					}
				});
				
				if (location != null) {
					flickr.PhotosGeoSetLocationAsync (
						result.Result,
						location.Latitude,
						location.Longitude,
						locResult => {
						Console.WriteLine ("Uploaded the image location too");	
					});
				}
			});
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
			
			StyleApp ();
			
			
			PerformSelector (new Selector ("UpdateLocation"), null, 1.0f);
									
			captureButton.TouchUpInside += (sender, e) => {				
				var flickr = FlickrManager.GetAuthInstance ();
				
				if (!flickr.IsAuthenticated) {				
					var avc = new AuthenticationViewController ();
					
					avc.AuthenticationCompleted += token => {
						Console.WriteLine ("Token came back as + " + token);
						this.DismissModalViewControllerAnimated (true);
						TakeAPhoto ();
					};
					
					this.PresentModalViewController (avc, true);
				} else {
					TakeAPhoto ();
				}
			};
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		private void StyleApp ()
		{
			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.BlackTranslucent;
			
			captureButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			captureButton.SetBackgroundImage (new UIImage ("Images/blue-button.png"), UIControlState.Normal);
			
			navigationBar.SetBackgroundImage (new UIImage ("Images/menu-bar.png"), UIBarMetrics.Default);
			
			View.BackgroundColor = UIColor.FromPatternImage (new UIImage ("Images/bg-gunmetal.png"));
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
	}
}

