using Windows.UI.Xaml.Navigation;

namespace MobileApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            LoadApplication(new MobileApp.App());
        }
    }
}