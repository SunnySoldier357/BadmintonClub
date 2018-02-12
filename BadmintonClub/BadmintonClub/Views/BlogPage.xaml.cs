using BadmintonClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlogPage : ContentPage
	{
        static BlogPostViewModel blogPostViewmodel;

        public BlogPage()
		{
            blogPostViewmodel = new BlogPostViewModel();
            InitializeComponent();
            MyListView.ItemsSource = blogPostViewmodel.BlogPostCollection;
        }

        public void AddBlogPostButton_Clicked(object sender, EventArgs e)
        {
            if (App.SignedInUser.IsAdmin())
            {
                // Pop up or new page to add new blogpost
            }

        }
    }
}