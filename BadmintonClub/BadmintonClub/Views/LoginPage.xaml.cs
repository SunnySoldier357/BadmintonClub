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
            resetErrorLabels();
            if (!(FullNameEntry.Text == null || LogInPasswordEntry.Text == null))
            {
                if (await azureService.LoginAsync(FullNameEntry.Text, LogInPasswordEntry.Text))
                    (Application.Current as App).StartMainApplication();
                else
                    showLabel(LogInErrorLabel, "The name or password entered was incorrect. If you have never signed-up, please sign up first.");
            }
            else
                showLabel(LogInErrorLabel, "Do not leave the fields empty!");
        }

        public async Task SignUpButton_ClickedAsync(object sender, EventArgs e)
        {
            resetErrorLabels();

            if (!(LastNameEntry.Text == null || FirstNameEntry.Text == null 
                || SignUpPasswordEntry.Text == null || ClubPINEntry.Text == null))
            {
                if (ClubPINEntry.Text?.Equals("testPIN") ?? false)
                {
                    if (await azureService.DoesUserExistAsync(FirstNameEntry.Text, LastNameEntry.Text))
                        showLabel(SignUpErrorLabel, "User already exists! Please sign in!");
                    else
                    {
                        (Application.Current as App).SignedInUser = await azureService.AddUserAsync(FirstNameEntry.Text, LastNameEntry.Text, SignUpPasswordEntry.Text);
                        (Application.Current as App).SignedInUserId = (Application.Current as App).SignedInUser.Id;
                        (Application.Current as App).StartMainApplication();
                    }
                }
                else
                    showLabel(SignUpErrorLabel, "The PIN entered was wrong!");
            }
            else
                showLabel(SignUpErrorLabel, "Do not leave the fields empty!");
        }

        // Private Methods
        private void resetErrorLabels()
        {
            LogInErrorLabel.IsVisible = false;
            SignUpErrorLabel.IsVisible = false;

            LogInErrorLabel.Text = string.Empty;
            SignUpErrorLabel.Text = string.Empty;
        }

        private void showLabel(Label label, string message)
        {
            label.IsVisible = true;
            label.Text = message;

            LogInPasswordEntry.Text = string.Empty;
            SignUpPasswordEntry.Text = string.Empty;
            ClubPINEntry.Text = string.Empty;
        }
    }
}