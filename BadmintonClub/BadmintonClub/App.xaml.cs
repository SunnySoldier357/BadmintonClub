using BadmintonClub.ViewModels;
using BadmintonClub.Views;
using Xamarin.Forms;

namespace BadmintonClub
{
    public partial class App : Application
	{
        // Public Static Properties
        public UserViewModel UserVM { get; set; }

        // Constructor
        public App()
		{
			InitializeComponent();
            UserVM = new UserViewModel();

            MainPage = new MainPage();
        }

        // Event Handlers
        protected override void OnResume()
        {
            // Handle when your app resumes
        }

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}