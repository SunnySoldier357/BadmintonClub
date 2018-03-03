using BadmintonClub.Models.Data_Access_Layer;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

namespace BadmintonClub.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            LoadApplication(new BadmintonClub.App());
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Parameter is Uri)
            {
                var service = DependencyService.Get<AzureService>();
                service.Client.ResumeWithURL(e.Parameter as Uri);
            }
        }
    }
}
