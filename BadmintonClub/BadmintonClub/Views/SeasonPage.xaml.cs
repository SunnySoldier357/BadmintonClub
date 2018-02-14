using BadmintonClub.Models;
using BadmintonClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            SeasonTableListView.ItemsSource = userViewModel.UserCollection;
        }
    }
}