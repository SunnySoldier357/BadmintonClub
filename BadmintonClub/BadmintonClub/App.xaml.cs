using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using BadmintonClub.ViewModels;
using BadmintonClub.Views;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BadmintonClub
{
    public partial class App : Application
	{
        // Public Static Properties
        public static User SignedInUser { get; set; }

        public static UserViewModel UserVM { get; set; }

        // Constructor
        public App()
		{
			InitializeComponent();
            Properties.Add("SignedInUser", new User("Sandeep", "Singh Sidhu", "Co-president", 2));
            Properties.Clear();
            UserVM = new UserViewModel();
            if (!Properties.ContainsKey("SignedInUser"))
                MainPage = new LoginPage();
            else
            {
                SignedInUser = (User)Properties["SignedInUser"];
                MainPage = new MainPage();
            }
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

        // Public Methods
        public void StartMainApplication()
        {
            MainPage = new MainPage();
        }
    }
}