using BadmintonClub.Models.Data_Access_Layer;
using Plugin.Connectivity;
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
                if (await azureService.DoesUserExistAsync(FullNameEntry.Text.Trim()))
                {
                    if (await azureService.LoginAsync(FullNameEntry.Text.Trim(), LogInPasswordEntry.Text.Trim()))
                        (Application.Current as App).StartMainApplication();
                    else
                        showLabel(LogInErrorLabel, "The name or password entered was incorrect.");
                }
                else
                    showLabel(LogInErrorLabel, "User does not exist. Please sign up!");
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
                    if (await azureService.DoesUserExistAsync(FirstNameEntry.Text.Trim() + " " +  LastNameEntry.Text.Trim()))
                        showLabel(SignUpErrorLabel, "User already exists! Please sign in!");
                    else
                    {
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            (Application.Current as App).SignedInUser = await azureService.AddUserAsync(FirstNameEntry.Text.Trim(), LastNameEntry.Text.Trim(), SignUpPasswordEntry.Text.Trim());
                            (Application.Current as App).SignedInUserId = (Application.Current as App).SignedInUser.Id;
                            (Application.Current as App).StartMainApplication();
                        }
                        else
                            showLabel(SignUpErrorLabel, "Device is offline. Sign-up is only available when device is online.");
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