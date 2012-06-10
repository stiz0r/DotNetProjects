using System;
using System.Drawing;

using MonoTouch.Twitter;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TwitterApp
{
	public partial class TwitterAppViewController : UIViewController
	{
		public TwitterAppViewController () : base ("TwitterAppViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren'Console.WriteLine("Cancelled the tweet");t in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			UIButton tweetButton = UIButton.FromType (UIButtonType.RoundedRect);
			tweetButton.Frame = new RectangleF (0, 0, 300f, 40f);
			
			tweetButton.SetTitle ("Tweet", UIControlState.Normal);
			tweetButton.TouchUpInside += (sender, e) => {
				var tvc = new TWTweetComposeViewController ();
				tvc.SetInitialText ("Learing about Monothouch at the #NDCOslo workshop!");
				tvc.SetCompletionHandler ((result) => 
				{
					if (result == TWTweetComposeViewControllerResult.Cancelled) {
						Console.WriteLine ("Cancelled the tweet");
					} else {
						Console.WriteLine ("Tweet sent! Hurrah!");
					}
					this.DismissModalViewControllerAnimated (true);
				}
				);
				this.PresentModalViewController (tvc, true);
			};
			View.AddSubview (tweetButton);
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
	}
}

