using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using Microsoft.AppCenter.Crashes;
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
    public class BlogPostViewModel : BaseViewModel
    {
        // Private Properties
        private bool addingNewItem;

        private GridLength listViewColumnWidth;
        private GridLength newItemColumnWidth;

        private ICommand addBlogPostCommand;
        private ICommand loadBlogPostsCommand;

        private string loadingMessage;

        // Public Properties
        public bool AddingNewItem
        {
            get => addingNewItem;
            set => SetProperty(ref addingNewItem, value);
        }
        public bool NotAddingNewItem { get => !addingNewItem; }

        public GridLength ListViewColumnWidth
        {
            get => listViewColumnWidth;
            set => SetProperty(ref listViewColumnWidth, value);
        }
        public GridLength NewItemColumnWidth
        {
            get => newItemColumnWidth;
            set => SetProperty(ref newItemColumnWidth, value);
        }

        public ICommand AddBlogPostCommand => addBlogPostCommand ?? (addBlogPostCommand = 
            new Command(async (dynamic arguments) => await executeAddBlogPostCommandAsync(arguments)));
        public ICommand LoadBlogPostsCommand => loadBlogPostsCommand ?? (loadBlogPostsCommand = 
            new Command(async () => await executeLoadBlogPostsCommandAsync()));

        public ObservableRangeCollection<BlogPost> BlogPosts { get; }
        public ObservableRangeCollection<BlogPost> BlogPostSorted { get; }

        public string LoadingMessage
        {
            get => loadingMessage;
            set => SetProperty(ref loadingMessage, value);
        }

        // Constructors
        public BlogPostViewModel()
        {
            addingNewItem = false;

            listViewColumnWidth = GridLength.Star;
            newItemColumnWidth = 0;

            BlogPosts = new ObservableRangeCollection<BlogPost>();
            BlogPostSorted = new ObservableRangeCollection<BlogPost>();
        }

        // Private Methods
        private async Task executeAddBlogPostCommandAsync(dynamic arguments)
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Adding new Blog Post...";
                IsBusy = true;

                AzureTransaction azureTransaction = new AzureTransaction(
                    new Transaction(arguments, TransactionType.AddBlogPost));

                var blogpost = (await azureTransaction.ExecuteAsync())[0] as BlogPost;
                BlogPosts.Add(blogpost);

                sortBlogPosts();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    { "Location", "BlogPostViewModel.executeAddBlogPostCommandAsync()" },
                });
                Debug.WriteLine("OH NO! " + ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }
        }

        private async Task executeLoadBlogPostsCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Loading Blog Posts...";
                IsBusy = true;

                AzureTransaction azureTransaction = new AzureTransaction(new Transaction(null, TransactionType.GetBlogPosts));

                var blogposts = (await azureTransaction.ExecuteAsync())[0] as List<BlogPost>;
                BlogPosts.ReplaceRange(blogposts);

                sortBlogPosts();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    { "Location", "BlogPostViewModel.executeLoadBlogPostsCommandAsync()" },
                });
                Debug.WriteLine("\nOH NO! " + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync blog posts, you may be offline", "OK");
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;

                FinishLoadingDel();
            }
        }

        private void sortBlogPosts()
        {
            LoadingMessage = "Sorting Blog Posts...";

            var data = from bp in BlogPosts
                       orderby bp.DateTimePublished descending
                       select bp;

            BlogPostSorted.ReplaceRange(data);
        }
    }
}