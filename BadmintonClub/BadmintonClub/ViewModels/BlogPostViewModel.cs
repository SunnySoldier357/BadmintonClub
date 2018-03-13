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
    public class BlogPostViewModel : BaseViewModel
    {
        // Private Properties
        private AzureService azureService;

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

        public ICommand AddBlogPostCommand =>addBlogPostCommand ?? (addBlogPostCommand = 
            new Command(async (dynamic arguments) => await executeAddBlogPostCommandAsync(arguments)));
        public ICommand LoadBlogPostsCommand =>loadBlogPostsCommand ?? (loadBlogPostsCommand = 
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
            azureService = AzureService.DefaultService;

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

                var blogpost = await azureService.AddBlogPostAsync(arguments.BlogTitle, arguments.BodyOfPost);
                BlogPosts.Add(blogpost);
                sortBlogPosts();
            }
            catch (Exception ex)
            {
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
                Stopwatch stopwatch = Stopwatch.StartNew();

                LoadingMessage = "Loading Blog Posts...";
                IsBusy = true;

                Stopwatch stopwatch2 = Stopwatch.StartNew();
                AzureTransaction azureTransaction = new AzureTransaction(new Transaction(null, TransactionType.GetBlogPosts));

                //var blogposts = await azureService.GetBlogPostsAsync();
                var blogposts = (await azureTransaction.ExecuteAsync()) as IEnumerable<BlogPost>;
                stopwatch2.Stop();
                Debug.WriteLine(string.Format("\n  >>>>> Time taken to load BlogPosts in AzureTransaction: {0} ms\n", stopwatch.ElapsedMilliseconds));
                Debug.WriteLine("blogpost Count: " + blogposts.Count());
                foreach (var item in blogposts)
                {
                    Debug.WriteLine(item.Publisher.ToString());
                }
                //BlogPosts.ReplaceRange(blogposts);
                BlogPosts.Clear();
                Debug.WriteLine("In BlogPostViewModel: ");
                foreach (var item in blogposts)
                {
                    Debug.WriteLine(item.Publisher?.ToString() ?? "null!!!");
                    BlogPosts.Add(new BlogPost()
                    {
                        Title = item.Title,
                        BodyOfPost = item.BodyOfPost,
                        DateTimePublished = item.DateTimePublished,
                        Id = item.Id,
                        UserID = item.UserID,
                        Publisher = item.Publisher
                    });
                }

                sortBlogPosts();
                stopwatch.Stop();
                Debug.WriteLine(string.Format("\n  >>>>> Time taken to load BlogPosts in BlogPostViewModel: {0} ms\n", stopwatch.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
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