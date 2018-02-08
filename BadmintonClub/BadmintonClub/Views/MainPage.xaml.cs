using BadmintonClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BadmintonClub
{
	public partial class MainPage : ContentPage
	{
        static BlogPostViewModel blogPostViewmodel;

		public MainPage()
		{
            blogPostViewmodel = new BlogPostViewModel();
			InitializeComponent();
            MyListView.ItemsSource = blogPostViewmodel.BlogPostCollection;
        }
	}
}
