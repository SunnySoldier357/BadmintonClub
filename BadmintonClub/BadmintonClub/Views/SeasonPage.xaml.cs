using BadmintonClub.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeasonPage : TabbedPage
    {
        static UserViewModel userViewModel = new UserViewModel();

        public SeasonPage ()
        {
            InitializeComponent();

            SeasonTableListView.ItemsSource = userViewModel.Users;
        }
    }
}