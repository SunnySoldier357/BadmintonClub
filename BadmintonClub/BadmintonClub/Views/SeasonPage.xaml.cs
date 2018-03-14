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
            seasonDataViewModel.AddMatchCommand.Execute(new
            {
                OpponentScore = OpponentScoreEntry.Text.Trim(),
                PlayerScore = PlayerScoreEntry.Text.Trim(),
                OpponentName = OpponentNamePicker.SelectedItem.ToString().Trim(),
                PlayerName = PlayerNamePicker.SelectedItem.ToString().Trim(),
            });
            switchToMainView();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            seasonDataViewModel.LoadSeasonDataCommand.Execute(null);
        }

        // Private Methods
        private void switchToEditView()
        {
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
        }

        private void switchToMainView()
        {
            seasonDataViewModel.AddingNewMatch = false;
            seasonDataViewModel.NewMatchColumnWidth = 0;
            seasonDataViewModel.ListViewColumnWidth = GridLength.Star;
        }
    }
}