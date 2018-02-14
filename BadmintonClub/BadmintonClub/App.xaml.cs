using BadmintonClub.Models;
using BadmintonClub.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BadmintonClub
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            //MainPage = new BlogPage();
            MainPage = new HamburgerNavigationPage();
        }

		protected override void OnStart ()
		{
            AppCenter.Start("ios=d719946d-1ee1-4cc0-9ddf-caaf7b004a2d;" +
                   "uwp=bd7c64cf-4481-41fc-ab41-e2d5d0e79cf3;" +
                   "android=a5c8d06b-6bb8-42f4-b5d4-13b92f500d97;",
                   typeof(Analytics), typeof(Crashes));
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
