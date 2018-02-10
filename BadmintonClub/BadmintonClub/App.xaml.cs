using BadmintonClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BadmintonClub
{
	public partial class App : Application
	{
        public User SignedInUser;

		public App ()
		{
			InitializeComponent();

			MainPage = new BadmintonClub.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
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
