﻿using BadmintonClub.Models;
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
        public static User SignedInUser = new User()
        {
            FirstName = "Test",
            LastName = "Name",
            Id = "1",
            ClearanceLevel = 2
        };

        // Private Properties
        private AzureService azureService;

        private ICommand addUserCommand;
        private ICommand loadUsersCommand;

        private string loadingMessage;

        // Public Properties
        public ObservableRangeCollection<User> Users { get; } 
            = new ObservableRangeCollection<User>();

        public ICommand AddUserCommand =>
            addUserCommand ?? (addUserCommand = new Command(async () => await executeAddUserCommandAsync()));
        public ICommand LoadUsersCommand =>
            loadUsersCommand ?? (loadUsersCommand = new Command(async () => await executeLoadUsersCommandAsync()));

        public string LoadingMessage
        {
            get => loadingMessage;
            set => SetProperty(ref loadingMessage, value);
        }

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
                LoadingMessage = "Adding New User...";
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
                LoadingMessage = "";
                IsBusy = false;
            }
        }

        public async Task executeLoadUsersCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Loading Users...";
                IsBusy = true;

                var users = await azureService.GetUsers();
                Users.ReplaceRange(users);
                Debug.WriteLine("/nItems in Users ({0}) vs from azureService ({1})", Users.Count(), users.Count());
                sortUsers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync users, you may be offline", "OK");
            }
            finally
            {
                LoadingMessage = "";
                IsBusy = false;
            }
        }

        private void sortUsers()
        {
            LoadingMessage = "Sorting Users...";

            var users = from user in Users
                        orderby user.PointsInCurrentSeason descending, user.FirstName
                        select user;

            Users.ReplaceRange(users);
            Debug.WriteLine("/nNumber of Users after sorting: {0}", Users.Count());
        }
    }
}
