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

            BlogPostListView.ItemTapped += (sender, e) =>
            {
                if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                    BlogPostListView.SelectedItem = null;
            };

            if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Command = blogPostViewmodel.LoadBlogPostsCommand
                });
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            blogPostViewmodel.LoadBlogPostsCommand.Execute(null);
        }
    }
}