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
        // Private Methods
        private AzureService azureService = AzureService.DefaultService;

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
            if (!(FullNameEntry.Text == null || LogInPasswordEntry.Text == null))
            {
                resetLabel(LogInErrorLabel);

                if (await azureService.LoginAsync(FullNameEntry.Text, LogInPasswordEntry.Text))
                    (Application.Current as App).StartMainApplication();
                else
                    showLabel(LogInErrorLabel, "The name or password entered was incorrect.");
            }
            else
                showLabel(LogInErrorLabel, "Do not leave the fields empty!");
        }

        public async Task SignUpButton_ClickedAsync(object sender, EventArgs e)
        {
            if (!(LastNameEntry.Text == null || FirstNameEntry.Text == null 
                || SignUpPasswordEntry.Text == null || ClubPINEntry.Text == null))
            {
                if (ClubPINEntry.Text?.Equals("testPIN") ?? false)
                {
                    resetLabel(SignUpErrorLabel);

                    (Application.Current as App).SignedInUser = await azureService.AddUserAsync(FirstNameEntry.Text, LastNameEntry.Text);
                    (Application.Current as App).SignedInUserId = (Application.Current as App).SignedInUser.Id;
                    (Application.Current as App).StartMainApplication();
                }
                else
                    showLabel(LogInErrorLabel, "The PIN entered was wrong!");
            }
            else
                showLabel(LogInErrorLabel, "Do not leave the fields empty!");
        }

        // Private Methods
        private void resetLabel(Label label)
        {
            label.IsVisible = false;
            label.Text = string.Empty;
        }

        private void showLabel(Label label, string message)
        {
            label.IsVisible = true;
            label.Text = message;
        }
    }
}