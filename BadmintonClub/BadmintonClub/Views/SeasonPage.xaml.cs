using BadmintonClub.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeasonPage : TabbedPage
    {
        // Private Properties
        private UserViewModel userViewModel;

        // Constructor
        public SeasonPage()
        {
            InitializeComponent();
            BindingContext = userViewModel = new UserViewModel();

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

        // Event Handlers\
        public void MatchCancelButton_Clicked(object sender, EventArgs e)
        {
            switchToMainView();
        }

        public void MatchSaveButton_Clicked(object sender, EventArgs e)
        {
            //blogPostViewModel.BlogTitle = BlogPostTitleEntry.Text;
            //blogPostViewModel.BodyOfPost = BlogPostBodyEditor.Text;
            //userViewModel.AddMatchCommand.Execute({ 3, 3, 3, 3});
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