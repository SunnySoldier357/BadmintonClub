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
        }

        // Event Handlers
        protected override void OnAppearing()
        {
            base.OnAppearing();

            userViewModel.LoadUsersCommand.Execute(null);
        }
    }
}