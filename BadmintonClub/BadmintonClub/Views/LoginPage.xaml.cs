using BadmintonClub.Models.Data_Access_Layer;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        // Private Properties
        private bool authenticated = false;

        // Constructor
        public LoginPage()
		{
			InitializeComponent();

            // Padding for iOS to not cover status bar
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);
        }

        // Event Handlers
        public async Task LogInButton_ClickedAsync(object sender, EventArgs e)
        {
            AzureService azureService = DependencyService.Get<AzureService>();
            if (!(LastNameEntry.Text == null) || (FirstNameEntry.Text == null))
            {
                if (PINEntry.Text?.Equals("testPIN") ?? false)
                {
                    ErrorLabel.IsVisible = false;
                    ErrorLabel.Text = string.Empty;

                    (Application.Current as App).SignedInUser = await azureService.AddUserAsync(FirstNameEntry.Text, LastNameEntry.Text);
                    (Application.Current as App).SignedInUserId = (Application.Current as App).SignedInUser.Id;
                    (Application.Current as App).StartMainApplication();
                }
                else
                {
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.Text = "The PIN entered was wrong!";
                }
            }
            else
            {
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = "Do not leave the First Name and/or Last Name fields empty!";
            }
        }
    }
}