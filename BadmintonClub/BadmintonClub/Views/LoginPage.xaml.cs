using BadmintonClub.Models;
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
            showLoading("Logging in...");

            resetErrorLabels();
            if (!(string.IsNullOrWhiteSpace(FullNameEntry.Text) || string.IsNullOrEmpty(LogInPasswordEntry.Text)))
            {
                var arguments = new
                {
                    FullName = formatName(FullNameEntry.Text),
                    Password = LogInPasswordEntry.Text
                };
                AzureTransaction azureTransaction = new AzureTransaction(
                    new Transaction(arguments, TransactionType.LogIn));
                var result = await azureTransaction.ExecuteAsync() as object[];
                
                if (result[0] == null)
                    showLabel(LogInErrorLabel, result[1].ToString());
                else
                {
                    if (bool.Parse(result[0].ToString()))
                    {
                        resetBindings();
                        (Application.Current as App).StartMainApplication();
                    }
                    else
                        showLabel(LogInErrorLabel, result[1].ToString());
                }
            }
            else
                showLabel(LogInErrorLabel, "Do not leave the fields empty!");

            resetBindings();
        }

        public async Task SignUpButton_ClickedAsync(object sender, EventArgs e)
        {
            showLoading("Signing up...");

            resetErrorLabels();

            if (!(string.IsNullOrWhiteSpace(LastNameEntry.Text) || string.IsNullOrWhiteSpace(FirstNameEntry.Text)
                || string.IsNullOrEmpty(SignUpPasswordEntry.Text) || string.IsNullOrWhiteSpace(ClubPINEntry.Text)))
            {
                if (ClubPINEntry.Text?.Equals("shsbadmintonclub0110") ?? false)
                {
                    var arguments = new
                    {
                        FirstName = formatName(FirstNameEntry.Text),
                        LastName = formatName(LastNameEntry.Text),
                        Password = SignUpPasswordEntry.Text,
                        IsCompetitive = CompetitivePlaySwitch.IsToggled
                    };
                    AzureTransaction azureTransaction = new AzureTransaction(
                        new Transaction(arguments, TransactionType.SignUp));
                    var result = await azureTransaction.ExecuteAsync() as object[];

                    if (result[0] == null)
                        showLabel(SignUpErrorLabel, result[1].ToString());
                    else
                    {
                        User resultUser = result[0] as User;
                        (Application.Current as App).SignedInUser = resultUser;
                        (Application.Current as App).SignedInUserId = resultUser.Id;

                        resetBindings();

                        (Application.Current as App).StartMainApplication();
                    }
                }
                else
                    showLabel(SignUpErrorLabel, "The PIN entered was wrong!");
            }
            else
                showLabel(SignUpErrorLabel, "Do not leave the fields empty!");

            resetBindings();
        }

        // Private Methods
        private string formatName(string name)
        {
            string result = string.Empty;
            string cleanedString = System.Text.RegularExpressions.Regex.Replace(name, @"\s+", " ");

            foreach (string item in cleanedString.Trim().Split(' '))
                result += item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower() + " ";

            return result.Trim();
        }

        private void resetBindings()
        {
            LoadingStackLayout.IsVisible = false;
            LoadingActivityIndicator.IsRunning = false;
            LoadingMessageLabel.Text = string.Empty;
            LogInButton.IsEnabled = true;
            SignUpButton.IsEnabled = true;
        }

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
            label.Text = "Error! " + message;

            LogInPasswordEntry.Text = string.Empty;
            SignUpPasswordEntry.Text = string.Empty;
            ClubPINEntry.Text = string.Empty;
        }

        private void showLoading(string loadingMessage)
        {
            LoadingStackLayout.IsVisible = true;
            LoadingActivityIndicator.IsRunning = true;
            LoadingMessageLabel.Text = loadingMessage;
            LogInButton.IsEnabled = false;
            SignUpButton.IsEnabled = false;
        }
    }
}