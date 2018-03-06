using BadmintonClub.Models.Data_Access_Layer;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
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
            LoadApplication(new BadmintonClub.App());
        }

        // Event Handler
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Parameter is Uri)
            {
                var service = DependencyService.Get<AzureService>();
                service.Client.ResumeWithURL(e.Parameter as Uri);
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
                    user = await ToDoItemManager
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
