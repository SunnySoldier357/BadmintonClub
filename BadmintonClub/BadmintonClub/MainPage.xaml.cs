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

            ListView MyListView = new ListView
            {
                // Source of data items
                ItemsSource = blogPostViewmodel.BlogPostCollection,

                // Template for each item
                ItemTemplate = new DataTemplate(() =>
                {
                    Label titleLabel = new Label();
                    titleLabel.SetBinding(Label.TextProperty, "Title");

                    Label publishmentDetailsLabel = new Label();
                    publishmentDetailsLabel.SetBinding(Label.TextProperty, "DateTimePublishedString");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Children =
                            {
                                titleLabel,
                                publishmentDetailsLabel
                            }
                        }
                    };
                })
            };

            // Build the Page
            this.Content = new StackLayout
            {
                Children =
                {
                    MyListView
                }
            };
        }
	}
}
