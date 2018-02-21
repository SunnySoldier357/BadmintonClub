using BadmintonClub.ViewModels;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlogPage : ContentPage
	{
        private BlogPostViewModel blogPostViewmodel;

        public BlogPage()
		{
            InitializeComponent();
            BindingContext = blogPostViewmodel = new BlogPostViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            blogPostViewmodel.LoadBlogPostsCommand.Execute(null);
        }
    }
}