using BadmintonClub.Models;
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
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlogPage : ContentPage
	{
        static BlogPostViewModel blogPostViewmodel;

        public BlogPage()
		{
            InitializeComponent();

            blogPostViewmodel = new BlogPostViewModel();
            MyListView.ItemsSource = blogPostViewmodel.BlogPostCollection;
        }
    }
}