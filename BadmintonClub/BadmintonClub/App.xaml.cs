using BadmintonClub.Models;
using BadmintonClub.Views;
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
		}

        protected override void OnStart()
        {
            // Handle when your app starts
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
            if (!Properties.ContainsKey("SignedInUserId"))
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