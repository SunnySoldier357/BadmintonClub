using BadmintonClub.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeasonPage : TabbedPage
    {
        // Public Properties
        UserViewModel userViewModel = new UserViewModel();

        // Constructor
        public SeasonPage()
        {
            InitializeComponent();
        }
    }
}