using BadmintonClub.Models;
using BadmintonClub.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        // Private Properties
        private User signedInUser = (Application.Current as App).SignedInUser;

        private UserViewModel userViewModel;

        // Constructor
		public ProfilePage()
		{
			InitializeComponent();
            BindingContext = signedInUser;

            // Padding for iOS to not cover status bar
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);
        }
    }
}