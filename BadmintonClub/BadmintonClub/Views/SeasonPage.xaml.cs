using BadmintonClub.Models;
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
        static ObservableCollection<User> Users = new ObservableCollection<User>();

        public SeasonPage ()
        {
            InitializeComponent();

            SeasonTableListView.ItemsSource = Users;
        }
    }
}