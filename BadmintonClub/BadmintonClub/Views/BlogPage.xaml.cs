﻿using BadmintonClub.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static BadmintonClub.App;

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

            FinishLoadingDel = adminAppPostButton;
            blogPostViewModel.LoadBlogPostsCommand.Execute(null);

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
        }

        // Event Handlers
        public void BlogPostCancelButton_Clicked(object sender, EventArgs e)
        {
            switchToMainView();
        }

        public void BlogPostSaveButton_Clicked(object sender, EventArgs e)
        {
            resetErrorLabel();

            if (string.IsNullOrWhiteSpace(BlogPostTitleEntry.Text) || string.IsNullOrWhiteSpace(BlogPostBodyEditor.Text))
            {
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = "Error! Do not leave the fields empty!";
            }
            else
            {
                blogPostViewModel.AddBlogPostCommand.Execute(new
                {
                    BlogTitle = BlogPostTitleEntry.Text.Trim(),
                    BodyOfPost = BlogPostBodyEditor.Text.Trim()
                });
                switchToMainView();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            blogPostViewModel.LoadBlogPostsCommand.Execute(null);
        }

        // Private Methods
        private void adminAppPostButton()
        {
            if ((Application.Current as App).SignedInUser?.IsAdmin() ?? false)
            {
                ToolbarItem addItem = new ToolbarItem
                {
                    Text = "Add Post",
                    Command = new Command(() => switchToEditView()),
                    Icon = "add.png"
                };

                if ((Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS) && 
                    ToolbarItems.Count == 0)
                    ToolbarItems.Add(addItem);
                else if (Device.RuntimePlatform == Device.UWP && ToolbarItems.Count == 1)
                    ToolbarItems.Add(addItem);
            }
        }

        private void resetErrorLabel()
        {
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;
        }

        private void switchToMainView()
        {
            blogPostViewModel.AddingNewItem = false;
            blogPostViewModel.NewItemColumnWidth = 0;
            blogPostViewModel.ListViewColumnWidth = GridLength.Star;
        }

        private void switchToEditView()
        {
            blogPostViewModel.AddingNewItem = true;
            blogPostViewModel.NewItemColumnWidth = GridLength.Star;
            if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Width >= 1000)
                blogPostViewModel.ListViewColumnWidth = GridLength.Star;
            else
                blogPostViewModel.ListViewColumnWidth = 0;

            BlogPostTitleEntry.Text = string.Empty;
            BlogPostBodyEditor.Text = string.Empty;
            resetErrorLabel();
        }
    }
}