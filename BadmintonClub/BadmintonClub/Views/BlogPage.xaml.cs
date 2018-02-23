using BadmintonClub.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlogPage : ContentPage
	{
        // Private Properties
        private BlogPostViewModel blogPostViewmodel;

        // Constructor
        public BlogPage()
		{
            InitializeComponent();
            BindingContext = blogPostViewmodel = new BlogPostViewModel();

            BlogPostListView.ItemTapped += (sender, e) =>
            {
                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                    BlogPostListView.SelectedItem = null;
            };

            if (Device.RuntimePlatform != Device.iOS && Device.RuntimePlatform != Device.Android)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Command = blogPostViewmodel.LoadBlogPostsCommand
                });
            }
        }

        // Event Handlers
        protected override void OnAppearing()
        {
            base.OnAppearing();

            blogPostViewmodel.LoadBlogPostsCommand.Execute(null);
        }
    }
}