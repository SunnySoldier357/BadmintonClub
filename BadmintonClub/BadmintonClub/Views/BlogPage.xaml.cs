using BadmintonClub.ViewModels;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
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
                    blogPostViewmodel.AddingNewItem = true;
                    blogPostViewmodel.NewItemColumnWidth = GridLength.Star;
                    if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Width >= 1000)
                        blogPostViewmodel.ListViewColumnWidth = GridLength.Star;
                    else
                    {
                        blogPostViewmodel.ListViewColumnWidth = 0;
                    }
                }),
                Icon = "add.png"
            });

        }

        // Event Handlers
        protected override void OnAppearing()
        {
            base.OnAppearing();

            blogPostViewmodel.LoadBlogPostsCommand.Execute(null);
        }
    }
}