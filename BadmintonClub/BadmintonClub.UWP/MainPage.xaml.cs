using BadmintonClub.Models.Data_Access_Layer;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

namespace BadmintonClub.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        // Private Properties
        private MobileServiceUser user;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            // Initialise the authenticator before loading the app
            BadmintonClub.App.Init(this);
            LoadApplication(new BadmintonClub.App());
        }

        // Event Handler
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Parameter is Uri)
            {
                var service = DependencyService.Get<AzureService>();
                service.CurrentClient.ResumeWithURL(e.Parameter as Uri);
            }
        }

        // Interface Implementations
        public async Task<bool> Authenticate()
        {
            string message = string.Empty;
            bool success = false;

            try
            {
                // Sign in with Microsoft using a server-manager flow
                if (user==null)
                {
                    user = await AzureService.DefaultManager.CurrentClient.LoginAsync(
                        MobileServiceAuthenticationProvider.MicrosoftAccount, 
                        "SkylineHighSchoolBadmintonClub://easyauth.callback");

                    if (user != null)
                    {
                        success = true;
                        message = string.Format("You are signed-in as {0}.", user.UserId);
                    }
                }
            }
            catch (Exception ex)
            {
                message = string.Format("Authentication Failed: {0}", ex.Message);
            }

            // Display the success or failure message
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return success;
        }
    }
}
