using BadmintonClub.Models.Data_Access_Layer;
using BadmintonClub.ViewModels;
using System;
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
                    Command = blogPostViewmodel.LoadBlogPostsCommand,
                    Icon = "refresh.png"
                });
            }

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Add Post",
                Command = new Command(() => 
                {
                    switchToEditView();
                }),
                Icon = "add.png"
            });

        }

        // Event Handlers
        public void BlogPostCancelButton_Clicked(object sender, EventArgs e)
        {
            switchToMainView();
        }

        public void BlogPostSaveButton_Clicked(object sender, EventArgs e)
        {
            blogPostViewmodel.BlogTitle = BlogPostTitleEntry.Text;
            blogPostViewmodel.BodyOfPost = BlogPostBodyEditor.Text;
            blogPostViewmodel.AddBlogPostCommand.Execute(null);
            switchToMainView();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            blogPostViewmodel.LoadBlogPostsCommand.Execute(null);
        }

        // Private Methods
        private void switchToMainView()
        {
            blogPostViewmodel.AddingNewItem = false;
            blogPostViewmodel.NewItemColumnWidth = 0;
            blogPostViewmodel.ListViewColumnWidth = GridLength.Star;

            BlogPostTitleEntry.Text = "";
            BlogPostBodyEditor.Text = "";
        }

        private void switchToEditView()
        {
            blogPostViewmodel.AddingNewItem = true;
            blogPostViewmodel.NewItemColumnWidth = GridLength.Star;
            if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Width >= 1000)
                blogPostViewmodel.ListViewColumnWidth = GridLength.Star;
            else
                blogPostViewmodel.ListViewColumnWidth = 0;
        }
    }
}