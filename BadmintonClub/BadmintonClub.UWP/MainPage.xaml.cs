using Windows.UI.Xaml.Navigation;

namespace BadmintonClub.UWP
{
    public sealed partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            LoadApplication(new BadmintonClub.App());
        }
    }
}
