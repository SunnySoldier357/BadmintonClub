using BadmintonClub.ViewModels;
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
                    Command = userViewModel.AddMatchCommand,
                    Icon = "add.png"
                });
            }
        }

        // Event Handlers
        protected override void OnAppearing()
        {
            base.OnAppearing();

            userViewModel.LoadUsersCommand.Execute(null);
        }
    }
}