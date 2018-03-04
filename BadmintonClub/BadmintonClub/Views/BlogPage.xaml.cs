﻿using BadmintonClub.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlogPage : ContentPage
	{
        // Private Properties
        private BlogPostViewModel blogPostViewModel;

        // Constructor
        public BlogPage()
		{
            InitializeComponent();
            BindingContext = blogPostViewModel = new BlogPostViewModel();

            // Padding for iOS to not cover status bar
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

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
                    Command = blogPostViewModel.LoadBlogPostsCommand,
                    Icon = "refresh.png"
                });
            }

            if ((Application.Current as App).SignedInUser.IsAdmin())
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Add Post",
                    Command = new Command(() => switchToEditView(false)),
                    Icon = "add.png"
                });
            }
        }

        // Event Handlers
        public void BlogPostCancelButton_Clicked(object sender, EventArgs e)
        {
            switchToMainView();
        }

        public void BlogPostSaveButton_Clicked(object sender, EventArgs e)
        {
            blogPostViewModel.AddBlogPostCommand.Execute(new
            {
                BlogTitle = BlogPostTitleEntry.Text,
                BodyOfPost = BlogPostBodyEditor.Text
            });
            switchToMainView();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            blogPostViewModel.LoadBlogPostsCommand.Execute(null);
        }

        // Private Methods
        private void switchToMainView()
        {
            blogPostViewModel.AddingNewItem = false;
            blogPostViewModel.NewItemColumnWidth = 0;
            blogPostViewModel.ListViewColumnWidth = GridLength.Star;
        }

        private void switchToEditView(bool editing)
        {
            blogPostViewModel.AddingNewItem = true;
            blogPostViewModel.NewItemColumnWidth = GridLength.Star;
            if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Width >= 1000)
                blogPostViewModel.ListViewColumnWidth = GridLength.Star;
            else
                blogPostViewModel.ListViewColumnWidth = 0;

            //BlogPostTitleEntry.Text = editing ? blogPostViewModel.BlogTitle : "";
            //BlogPostBodyEditor.Text = editing ? blogPostViewModel.BodyOfPost : "";
            BlogPostTitleEntry.Text = string.Empty;
            BlogPostBodyEditor.Text = string.Empty;
        }
    }
}