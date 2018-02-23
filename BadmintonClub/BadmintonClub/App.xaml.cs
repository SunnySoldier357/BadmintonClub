using BadmintonClub.Views;
using Xamarin.Forms;

namespace BadmintonClub
{
    public partial class App : Application
	{
        // Constructor
		public App()
		{
			InitializeComponent();

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