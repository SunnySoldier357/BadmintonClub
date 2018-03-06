using BadmintonClub.Models;
using BadmintonClub.ViewModels;
using BadmintonClub.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BadmintonClub
{
    // Interface
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }

    public partial class App : Application
	{
        // Static Properties
        public static FinishLoading FinishLoadingDel;  // Used for Admin Authentication

        public static IAuthenticate Authenticator { get; private set; }

        // Static Methods
        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        // Public Properties
        public string SignedInUserId { get; set; }

        public User SignedInUser { get; set; }

        public UserViewModel UserVM { get; set; }

        // Constructor
        public App()
		{
			InitializeComponent();
<<<<<<< HEAD
            //Properties["SignedInUserId"] = "5";
            Properties.Clear();
=======
            Properties["SignedInUserId"] = "1";

>>>>>>> parent of 3f95c3a... fixed name needing to refresh
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