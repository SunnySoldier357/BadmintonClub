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
        // Private Properties
        private AzureService azureService;

        private bool addingNewMatch;

        private GridLength listViewColumnWidth;
        private GridLength newMatchColumnWidth;

        private ICommand addMatchCommand;
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
        public ObservableRangeCollection<SeasonData> SeasonDatas { get; }
        public ObservableRangeCollection<string> UserNamesOpponent { get; }
        public ObservableRangeCollection<string> UserNamesPlayer { get; }
        public ObservableRangeCollection<User> Users { get; } 
        public ObservableRangeCollection<User> UserSorted { get; }

        public ICommand AddMatchCommand => addMatchCommand ?? (addMatchCommand = 
            new Command(async (dynamic arguments) => await executeAddMatchCommandAsync(arguments)));
        public ICommand LoadMatchesCommand =>loadMatchesCommand ?? (loadMatchesCommand = 
            new Command(async () => await executeLoadMatchesCommandAsync()));
        public ICommand LoadUsersCommand =>loadUsersCommand ?? (loadUsersCommand = 
            new Command(async () => await executeLoadUsersCommandAsync()));

        public string LoadingMessage
        {
            get => loadingMessage;
            set => SetProperty(ref loadingMessage, value);
        }

        // Constructors
        public UserViewModel()
        {
            azureService = AzureService.DefaultService;

            listViewColumnWidth = GridLength.Star;
            newMatchColumnWidth = 0;

            Matches = new ObservableRangeCollection<Match>();
            SeasonDatas = new ObservableRangeCollection<SeasonData>();
            UserNamesOpponent = new ObservableRangeCollection<string>();
            UserNamesPlayer = new ObservableRangeCollection<string>();
            Users = new ObservableRangeCollection<User>();
            UserSorted = new ObservableRangeCollection<User>();
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

                string playerId = await azureService.GetUserIdFromNameAsync(arguments.PlayerName);
                string opponentId = await azureService.GetUserIdFromNameAsync(arguments.OpponentName);

                var match = await azureService.AddMatchAsync(int.Parse(arguments.PlayerScore), int.Parse(arguments.OpponentScore), playerId, opponentId);
                Matches.Add(match);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }

            LoadUsersCommand.Execute(null);
        }

        private async Task executeLoadMatchesCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Loading Matches...";
                IsBusy = true;

                var matches = await azureService.GetMatchesAsync();
                Matches.RemoveRange(matches);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
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

                var users = await azureService.GetUsersAsync();
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
                LoadingMessage = string.Empty;
                IsBusy = false;
            }
        }

        private void sortUsers()
        {
            LoadingMessage = "Sorting Users...";

            var users = from user in Users
                        orderby user.PointDifference descending,
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