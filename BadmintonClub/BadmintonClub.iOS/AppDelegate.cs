﻿using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace BadmintonClub.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public static Func<NSUrl, bool> ResumeWithURL;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var tint = UIColor.FromRGB(30, 142, 128);
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(242, 197, 0); // Bar background
            UINavigationBar.Appearance.TintColor = tint; // Tint color of button items
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                Font = UIFont.FromName("Avenir-Medium", 17f),
                TextColor = UIColor.Black
            });

            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes
            {
                Font = UIFont.FromName("Avenir-Medium", 17f),
                ForegroundColor = UIColor.Black
            };

            UIBarButtonItem.Appearance.TintColor = tint; // Tint color of button items


            // NavigationBar Buttons 
            UIBarButtonItem.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                Font = UIFont.FromName("Avenir-Medium", 17f),
                TextColor = tint
            }, UIControlState.Normal);

            // TabBar
            UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                Font = UIFont.FromName("Avenir-Book", 10f)
            }, UIControlState.Normal);

            UITabBar.Appearance.TintColor = tint;
            UISwitch.Appearance.OnTintColor = tint;

            Forms.Init();

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return ResumeWithURL != null && ResumeWithURL(url);
        }
    }
}
