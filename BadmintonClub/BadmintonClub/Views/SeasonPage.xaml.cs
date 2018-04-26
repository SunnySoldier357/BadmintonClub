using BadmintonClub.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeasonPage : TabbedPage
    {
        // Private Properties
        private SeasonDataViewModel seasonDataViewModel;

        // Constructor
        public SeasonPage()
        {
            InitializeComponent();
            BindingContext = seasonDataViewModel = new SeasonDataViewModel();

            // Padding for iOS to not cover status bar
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            SeasonTableListView.ItemTapped += (sender, e) =>
            {
                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                    SeasonTableListView.SelectedItem = null;
            };

            if (Device.RuntimePlatform != Device.iOS && Device.RuntimePlatform != Device.Android)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Command = seasonDataViewModel.LoadSeasonDataCommand,
                    Icon = "refresh.png"
                });
            }

            if ((Application.Current as App).SignedInUser.IsAdmin())
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Add Match",
                    Command = new Command(() => switchToEditView()),
                    Icon = "add.png"
                });
            }
        }

        // Event Handlers
        public void MatchCancelButton_Clicked(object sender, EventArgs e)
        {
            switchToMainView();
        }

        public void MatchSaveButton_Clicked(object sender, EventArgs e)
        {
            resetErrorLabel();

            if (OpponentNamePicker.SelectedItem == null || PlayerNamePicker.SelectedItem == null)
                showError("A Player and an opponent have to be picked!");
            else
            {
                if (OpponentNamePicker.SelectedItem.ToString().Equals(PlayerNamePicker.SelectedItem.ToString()))
                    showError("You cannot play yourself in a game!");
                else
                {
                    if (string.IsNullOrWhiteSpace(OpponentScoreEntry.Text) || string.IsNullOrWhiteSpace(PlayerScoreEntry.Text))
                        showError("Do not leave the fields empty!");
                    else
                    {
                        if (!(int.TryParse(OpponentScoreEntry.Text, out int x) && int.TryParse(PlayerScoreEntry.Text, out int y)))
                            showError("Score Entries only accept integers!");
                        else
                        {
                            seasonDataViewModel.AddMatchCommand.Execute(new
                            {
                                OpponentScore = OpponentScoreEntry.Text.Trim(),
                                PlayerScore = PlayerScoreEntry.Text.Trim(),
                                OpponentName = OpponentNamePicker.SelectedItem.ToString().Trim(),
                                PlayerName = PlayerNamePicker.SelectedItem.ToString().Trim(),
                                IsSeasonMatch = SeasonMatchSwitch.IsToggled,
                                StartNewSeason = NewSeasonSwitch.IsToggled
                            });
                            switchToMainView();
                        }
                    }
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            seasonDataViewModel.LoadSeasonDataCommand.Execute(null);
        }

        // Private Methods
        private void resetErrorLabel()
        {
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;
        }

        private void showError(string error)
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Error! " + error;
        }

        private void switchToEditView()
        {
            GlossaryStackLayout.IsVisible = false;
            seasonDataViewModel.AddingNewMatch = true;
            seasonDataViewModel.NewMatchColumnWidth = GridLength.Star;
            if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Width >= 1000)
                seasonDataViewModel.ListViewColumnWidth = GridLength.Star;
            else
                seasonDataViewModel.ListViewColumnWidth = 0;

            PlayerNamePicker.SelectedItem = null;
            OpponentNamePicker.SelectedItem = null;
            PlayerScoreEntry.Text = string.Empty;
            OpponentScoreEntry.Text = string.Empty;
            NewSeasonSwitch.IsToggled = false;
            SeasonMatchSwitch.IsToggled = false;
            resetErrorLabel();
        }

        private void switchToMainView()
        {
            GlossaryStackLayout.IsVisible = true;
            seasonDataViewModel.AddingNewMatch = false;
            seasonDataViewModel.NewMatchColumnWidth = 0;
            seasonDataViewModel.ListViewColumnWidth = GridLength.Star;
        }
    }
}