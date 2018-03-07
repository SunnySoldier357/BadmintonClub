using BadmintonClub.Models;
using BadmintonClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        // Private Properties
        private User signedInUser;

        private UserViewModel userViewModel;

        // Constructor
		public ProfilePage()
		{
			InitializeComponent();
            userViewModel = (Application.Current as App).UserVM;
            BindingContext = signedInUser = (Application.Current as App).SignedInUser;

            // Padding for iOS to not cover status bar
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);
        }
    }
}