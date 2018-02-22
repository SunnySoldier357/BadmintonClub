using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private ICommand addBlogPostCommand;
        private ICommand loadBlogPostsCommand;

        // Public Properties
        public ObservableRangeCollection<BlogPost> BlogPosts { get; }
        = new ObservableRangeCollection<BlogPost>();

        public ICommand AddBlogPostCommmand =>
            addBlogPostCommand ?? (addBlogPostCommand = new Command(async () => await executeAddBlogPostCommandAsync()));
        public ICommand LoadBlogPostsCommand =>
            loadBlogPostsCommand ?? (loadBlogPostsCommand = new Command(async () => await executeLoadBlogPostsCommandAsync()));

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
                IsBusy = true;

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 100; i++)
                    builder.Append(string.Format("This is a test blog {0}.", i));

                var blogpost = await azureService.AddBlogPost("Default Title", builder.ToString());
                BlogPosts.Add(blogpost);
                sortBlogPosts();
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

        private async Task executeLoadBlogPostsCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var blogposts = await azureService.GetBlogPosts();
                BlogPosts.ReplaceRange(blogposts);

                sortBlogPosts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                // await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync blog posts, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void sortBlogPosts()
        {
            var sorted = from bp in BlogPosts
                         orderby bp.DateTimePublished descending
                         select bp;
            Debug.WriteLine("Sortblogposts()");
            foreach (var item in sorted)
            {
                Debug.WriteLine(item.ToString());
            }

            if (sorted.Count() == 1)
                BlogPosts.Replace(sorted.First());
            else
                BlogPosts.ReplaceRange(sorted);
        }
    }
}
