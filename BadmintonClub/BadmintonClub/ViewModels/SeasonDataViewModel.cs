using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static BadmintonClub.App;

namespace BadmintonClub.ViewModels
{
    public class SeasonDataViewModel : BaseViewModel
    {
        // Private Properties
        private AzureService azureService;

        private bool addingNewMatch;

        private GridLength listViewColumnWidth;
        private GridLength newMatchColumnWidth;

        private ICommand addMatchCommand;
        private ICommand loadSeasonDataCommand;

        private string loadingMessage;

        // Public Properties
        public bool AddingNewMatch
        {
            get => addingNewMatch;
            set => SetProperty(ref addingNewMatch, value);
        }
        public bool NotAddingNewItem { get => !addingNewMatch; }

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

        public ICommand AddMatchCommand => addMatchCommand ?? (addMatchCommand =
            new Command(async (dynamic arguments) => await executeAddMatchCommandAsync(arguments)));
        public ICommand LoadSeasonDataCommand => loadSeasonDataCommand ?? (loadSeasonDataCommand =
            new Command(async () => await executeLoadSeasonDataCommandAsync()));

        public ObservableObject LargestPointDifferentialMatch { get; }

        public ObservableRangeCollection<SeasonData> SeasonDataCollection { get; }
        public ObservableRangeCollection<SeasonData> SeasonDataSorted { get; }
        public ObservableRangeCollection<string> UserNamesOpponent { get; }
        public ObservableRangeCollection<string> UserNamesPlayer { get; }

        public string LoadingMessage
        {
            get => loadingMessage;
            set => SetProperty(ref loadingMessage, value);
        }

        // Constructor
        public SeasonDataViewModel()
        {
            azureService = AzureService.DefaultService;

            addingNewMatch = false;

            listViewColumnWidth = GridLength.Star;
            newMatchColumnWidth = 0;

            LargestPointDifferentialMatch = new ObservableObject();

            SeasonDataCollection = new ObservableRangeCollection<SeasonData>();
            SeasonDataSorted = new ObservableRangeCollection<SeasonData>();
            UserNamesOpponent = new ObservableRangeCollection<string>();
            UserNamesPlayer = new ObservableRangeCollection<string>();
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

                AzureTransaction azureTransaction = new AzureTransaction(new Transaction(arguments, TransactionType.AddMatch));

                await azureTransaction.ExecuteAsync();
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

            LoadSeasonDataCommand.Execute(null);
        }

        private async Task executeLoadSeasonDataCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Loading Season Data...";
                IsBusy = true;

                AzureTransaction azureTransaction = new AzureTransaction(new Transaction(null, TransactionType.GetSeasonData));

                var seasonData = (await azureTransaction.ExecuteAsync())[0] as List<SeasonData>;

                SeasonDataCollection.ReplaceRange(seasonData);

                sortSeasonData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOH NO! " + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync season data, you may be offline", "OK");
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;

                FinishLoadingDel();
            }
        }

        private void sortSeasonData()
        {
            LoadingMessage = "Sorting Season Data...";

            var data = from season in SeasonDataCollection
                       orderby season.PointsInCurrentSeason descending,
                               season.PointDifference descending,
                               season.PointsFor descending,
                               season.Player.FirstName,
                               season.Player.LastName
                       select season;

            SeasonDataSorted.ReplaceRange(data);

            var usernames = from season in SeasonDataCollection
                            orderby season.Player.FirstName,
                                    season.Player.LastName
                            select season.Player.FullName;

            UserNamesPlayer.ReplaceRange(usernames);
            UserNamesOpponent.ReplaceRange(usernames);
        }
    }
}