using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Xaml.Navigation;

namespace BadmintonClub.UWP
{
    public sealed partial class MainPage
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
    }
}
