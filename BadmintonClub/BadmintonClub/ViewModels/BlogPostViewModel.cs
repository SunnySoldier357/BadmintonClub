using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BadmintonClub.ViewModels
{
    public class BlogPostViewModel : BaseViewModel
    {
        // Private Properties
        private AzureService azureService;

        private bool addingNewItem = false;

        private GridLength listViewColumnWidth = GridLength.Star;
        private GridLength newItemColumnWidth = 0;

        private ICommand addBlogPostCommand;
        private ICommand loadBlogPostsCommand;

        private string bodyOfPost;
        private string blogTitle;
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

        public ICommand AddBlogPostCommand =>
            addBlogPostCommand ?? (addBlogPostCommand = new Command(async () => await executeAddBlogPostCommandAsync()));
        public ICommand LoadBlogPostsCommand =>
            loadBlogPostsCommand ?? (loadBlogPostsCommand = new Command(async () => await executeLoadBlogPostsCommandAsync()));

        public ObservableRangeCollection<BlogPost> BlogPosts { get; }
            = new ObservableRangeCollection<BlogPost>();
        public ObservableRangeCollection<BlogPost> BlogPostSorted { get; }
            = new ObservableRangeCollection<BlogPost>();

        public string BodyOfPost
        {
            get => bodyOfPost;
            set => SetProperty(ref bodyOfPost, value);
        }
        public string BlogTitle
        {
            get => blogTitle;
            set => SetProperty(ref blogTitle, value);
        }
        public string LoadingMessage
        {
            get => loadingMessage;
            set => SetProperty(ref loadingMessage, value);
        }

        // Constructors
        public BlogPostViewModel()
        {
            azureService = DependencyService.Get<AzureService>();
        }

        // Private Methods
        private async Task executeAddBlogPostCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingMessage = "Adding new Blog Post...";
                IsBusy = true;

                var blogpost = await azureService.AddBlogPost(BlogTitle, BodyOfPost);
                BlogPosts.Add(blogpost);
                sortBlogPosts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO! " + ex);
            }
            finally
            {
                LoadingMessage = "";
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
                var blogposts = await azureService.GetBlogPosts();

                BlogPosts.Clear();
                foreach (var item in blogposts)
                {
                    BlogPosts.Add(new BlogPost()
                    {
                        Title = item.Title,
                        BodyOfPost = item.BodyOfPost,
                        DateTimePublished = item.DateTimePublished,
                        Publisher = await azureService.GetUser(item.UserID)
                    });
                }
                
                sortBlogPosts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOH NO! " + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync blog posts, you may be offline", "OK");
            }
            finally
            {
                LoadingMessage = "";
                IsBusy = false;
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