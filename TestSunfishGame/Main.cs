using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TestSunfishGame
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		TestGame game;

		public override void FinishedLaunching (UIApplication app)
		{
			// Fun begins..
			game = new TestGame ();
			game.Run ();

			//[[UIApplication sharedApplication] setStatusBarHidden:YES withAnimation:UIStatusBarAnimationSlide];
			//UIApplication.SharedApplication.SetStatusBarHidden (true, true);
			app.SetStatusBarHidden (true, true);
			var controller = UIApplication.SharedApplication.Windows[0].RootViewController;
			if (controller != null) {
				//controller.SetNeedsStatusBarAppearanceUpdate ();
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}

	}
}


