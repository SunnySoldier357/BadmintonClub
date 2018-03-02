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
        private UserViewModel userViewModel;

        // Public Properties
        public string test = "test";

        // Constructor
        public SeasonPage()
        {
            InitializeComponent();
            BindingContext = userViewModel = (Application.Current as App).UserVM;

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
                    Command = userViewModel.LoadUsersCommand,
                    Icon = "refresh.png"
                });
            }

            if (UserViewModel.SignedInUser.IsAdmin())
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
            userViewModel.AddMatchCommand.Execute(new
            {
                OpponentScore = OpponentScoreEntry.Text,
                PlayerScore = PlayerScoreEntry.Text,
                OpponentName = OpponentNamePicker.SelectedItem.ToString(),
                PlayerName = PlayerNamePicker.SelectedItem.ToString()
            });
            switchToMainView();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            userViewModel.LoadUsersCommand.Execute(null);
        }

        // Private Methods
        private void switchToEditView()
        {
            userViewModel.AddingNewMatch = true;
            userViewModel.NewMatchColumnWidth = GridLength.Star;
            if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Width >= 1000)
                userViewModel.ListViewColumnWidth = GridLength.Star;
            else
                userViewModel.ListViewColumnWidth = 0;
        }

        private void switchToMainView()
        {
            userViewModel.AddingNewMatch = false;
            userViewModel.NewMatchColumnWidth = 0;
            userViewModel.ListViewColumnWidth = GridLength.Star;
        }
    }
}