// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Norgegram.iOS
{
	[Register ("Norgegram_iOSViewController")]
	partial class Norgegram_iOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView imageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton captureButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UINavigationBar navigationBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imageView != null) {
				imageView.Dispose ();
				imageView = null;
			}

			if (captureButton != null) {
				captureButton.Dispose ();
				captureButton = null;
			}

			if (navigationBar != null) {
				navigationBar.Dispose ();
				navigationBar = null;
			}
		}
	}
}
