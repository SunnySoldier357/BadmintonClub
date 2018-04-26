using BadmintonClub.Models;
using BadmintonClub.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace BadmintonClub
{
    public partial class App : Application
	{
        // Static Properties
        public static FinishLoading FinishLoadingDel;  // Used for Admin Authentication

        // Public Properties
        public string SignedInUserId { get; set; } = string.Empty;

        public User SignedInUser { get; set; }

        // Constructor
        public App()
		{
			InitializeComponent();

            initialiseData();
        }

        // Event Handlers
        protected override void OnResume()
        {
            // Handle when your app resumes
        }

		protected override void OnSleep()
		{
            Properties["SignedInUserId"] = SignedInUserId;
            Current.SavePropertiesAsync();
		}

        protected override void OnStart()
        {
            AppCenter.Start("android=a5c8d06b-6bb8-42f4-b5d4-13b92f500d97;" +
                  "uwp=bd7c64cf-4481-41fc-ab41-e2d5d0e79cf3;" +
                  "ios=d719946d-1ee1-4cc0-9ddf-caaf7b004a2d",
                  typeof(Analytics), typeof(Crashes));
        }

        // Public Methods
        public void RestartApp()
        {
            SignedInUserId = string.Empty;
            Properties.Clear();
            initialiseData();
        }

        public void StartMainApplication()
        {
            MainPage = new MainPage();
        }

        // Private Methods
        private void initialiseData()
        {
            if (!Properties.ContainsKey("SignedInUserId") || string.IsNullOrWhiteSpace(Properties["SignedInUserId"].ToString()))
                MainPage = new LoginPage();
            else
            {
                SignedInUserId = Properties["SignedInUserId"].ToString();
                MainPage = new MainPage();
            }
        }

        // Classes. Structs, etc...
        public delegate void FinishLoading();
    }
}