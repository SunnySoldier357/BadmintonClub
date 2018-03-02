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
        public static User SignedInUser = new User()
        {
            FirstName = "Test",
            LastName = "Name",
            Id = "1",
            ClearanceLevel = 2
        };

        // Private Properties
        private AzureService azureService;

        private bool addingNewMatch;

        private GridLength listViewColumnWidth = GridLength.Star;
        private GridLength newMatchColumnWidth = 0;

        private ICommand addMatchCommand;
        private ICommand addUserCommand;
        private ICommand loadMatchesCommand;
        private ICommand loadUsersCommand;

        private string loadingMessage;

        // Public Properties
        public bool AddingNewMatch
        {
            get => addingNewMatch;
            set => SetProperty(ref addingNewMatch, value);
        }

        public GridLength ListViewColumnWidth
        {
            get => listViewColumnWidth;
            set => SetProperty(ref listViewColumnWidth, value);
        }
        public GridLength NewMatchColumnWidth
        {
            get => newMatchColumnWidth;
            set => SetProperty(ref newMatchColumnWidth, value);
        }

        public ObservableRangeCollection<Match> Matches { get; }
            = new ObservableRangeCollection<Match>();
        public ObservableRangeCollection<string> UserNamesOpponent { get; }
            = new ObservableRangeCollection<string>();
        public ObservableRangeCollection<string> UserNamesPlayer { get; }
            = new ObservableRangeCollection<string>();
        public ObservableRangeCollection<User> Users { get; } 
            = new ObservableRangeCollection<User>();
        public ObservableRangeCollection<User> UserSorted { get; }
            = new ObservableRangeCollection<User>();

        public ICommand AddMatchCommand =>
            addMatchCommand ?? (addMatchCommand = new Command(async (dynamic arguments) => await executeAddMatchCommandAsync(arguments)));
        public ICommand AddUserCommand =>
            addUserCommand ?? (addUserCommand = new Command(async () => await executeAddUserCommandAsync()));
        public ICommand LoadMatchesCommand =>
            loadMatchesCommand ?? (loadMatchesCommand = new Command(async () => await executeLoadMatchesCommandAsync()));
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
        private async Task executeAddMatchCommandAsync(dynamic arguments)
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Adding New Match...";
                IsBusy = true;

                string playerId = await azureService.GetUserIdFromName(arguments.PlayerName);
                string opponentId = await azureService.GetUserIdFromName(arguments.OpponentName);

                var match = await azureService.AddMatch(int.Parse(arguments.PlayerScore), int.Parse(arguments.OpponentScore), playerId, opponentId);
                Matches.Add(match);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                LoadingMessage = "";
                IsBusy = false;
            }

            LoadUsersCommand.Execute(null);
        }

        private async Task executeAddUserCommandAsync()
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

        private async Task executeLoadMatchesCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Loading Matches...";
                IsBusy = true;

                var matches = await azureService.GetMatches();
                Matches.RemoveRange(matches);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                LoadingMessage = "";
                IsBusy = false;
            }
        }

        private async Task executeLoadUsersCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Loading Users...";
                IsBusy = true;

                var users = await azureService.GetUsers();
                Users.ReplaceRange(users);

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
                        orderby user.PointsInCurrentSeason descending, 
                                user.PointDifference descending,
                                user.PointsFor descending,
                                user.FirstName,
                                user.LastName
                        select user;

            UserSorted.ReplaceRange(users);

            var usernames = from user in Users
                            orderby user.FirstName,
                                    user.LastName
                            select user.FullName;

            UserNamesPlayer.ReplaceRange(usernames);
            UserNamesOpponent.ReplaceRange(usernames);
        }
    }
}
