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
        public static User SignedInUser;

		public App ()
		{
			InitializeComponent();

			MainPage = new BlogPage();
            SignedInUser = new User("Sandeep", "Singh Sidhu", "Co-President", 2);
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
