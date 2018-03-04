using BadmintonClub.Models;
using BadmintonClub.ViewModels;
using BadmintonClub.Views;
using Xamarin.Forms;

namespace BadmintonClub
{
    public partial class App : Application
	{
        // Public Static Properties
        public static FinishLoading FinishLoadingDel;  // Used for BlogPage

        // Public Properties
        public string SignedInUserId { get; set; }

        public User SignedInUser { get; set; }

        public UserViewModel UserVM { get; set; }

        // Constructor
        public App()
		{
			InitializeComponent();

            UserVM = new UserViewModel();
            if (!Properties.ContainsKey("SignedInUserId"))
                MainPage = new LoginPage();
            else
            {
                SignedInUserId = Properties["SignedInUserId"].ToString();
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
            Properties["SignedInUserId"] = SignedInUserId;
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

        // Classes. Structs, etc...
        public delegate void FinishLoading();
    }
}