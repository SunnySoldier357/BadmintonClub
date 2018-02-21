using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BadmintonClub.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        // Public Static Properties
        public static User SignedInUser = new User();

        // Private Properties
        private AzureService azureService;

        private ICommand addUserCommand;
        private ICommand loadUsersCommand;

        // Public Properties
        public ObservableRangeCollection<User> Users { get; } 
            = new ObservableRangeCollection<User>();

        public ICommand AddUserCommand =>
            addUserCommand ?? (addUserCommand = new Command(async () => await executeAddUserCommandAsync()));
        public ICommand LoadUsersCommand =>
            loadUsersCommand ?? (loadUsersCommand = new Command(async () => await executeLoadUsersCommandAsync()));

        // Constructors
        public UserViewModel()
        {
            azureService = DependencyService.Get<AzureService>();
        }

        // Private Methods
        public async Task executeAddUserCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var user = await azureService.AddUser("Default", "Name");
                Users.Add(user);
                sortUsers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task executeLoadUsersCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var users = await azureService.GetUsers();
                Users.ReplaceRange(users);
                sortUsers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                // await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync users, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void sortUsers()
        {
            var users = from user in Users
                        orderby user.PointsInCurrentSeason descending, user.FirstName
                        select user;

            Users.ReplaceRange(users);
        }
    }
}
