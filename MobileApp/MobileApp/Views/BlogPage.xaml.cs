using MobileApp.Models.Data_Access_Layer;
using MobileApp.Models.DB;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlogPage : ContentPage
    {
        // Public Properties
        public ObservableCollection<BlogPost> BlogPosts = new ObservableCollection<BlogPost>();

        // Constructors
        public BlogPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        // Overridden Methods
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            AzureService azureService = AzureService.DefaultService;
            await azureService.SyncDataTablesAsync();

            var result = await azureService.GetBlogPostsAsync();

            foreach (var item in result)
                BlogPosts.Add(item);
        }
    }
}