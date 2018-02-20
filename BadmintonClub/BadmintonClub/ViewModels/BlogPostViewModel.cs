using BadmintonClub.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BadmintonClub.ViewModels
{
    public class BlogPostViewModel : INotifyPropertyChanged
    {
        // Private Properties
        //private BadmintonDAL bDAL;
        private ObservableCollection<BlogPost> blogPostCollection;

        // Public Properties
        public ObservableCollection<BlogPost> BlogPostCollection
        {
            get
            {
                blogPostCollection = new ObservableCollection<BlogPost>(from g in blogPostCollection
                                                                        orderby g.DateTimePublished descending
                                                                        select g);
                NotifyPropertyChanged();
                return blogPostCollection;
            }
            set { blogPostCollection = value; }
        }

        // Events and Event Handlers
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructors
        public BlogPostViewModel()
        {
            blogPostCollection = new ObservableCollection<BlogPost>();

            StringBuilder text = new StringBuilder();
            for (int i = 0; i < 100; i++)
                text.Append("This is a test blog.");

            User temp = new User("Sandeep", "Singh Sidhu", "Co-President", 2);
            AddBlogPost(new BlogPost("Update 10/2", new DateTime(2018, 2, 10, 7, 0, 0), text.ToString(), temp));
            AddBlogPost(new BlogPost("Update 25/1", new DateTime(2018, 1, 25, 7, 18, 0), text.ToString(), temp));
            AddBlogPost(new BlogPost("Update 20/2", new DateTime(2018, 2, 20, 13, 0, 0), text.ToString(), temp));
            AddBlogPost(new BlogPost("Update 15/2", new DateTime(2018, 2, 15, 7, 0, 0), text.ToString(), temp));
            AddBlogPost(new BlogPost("Update 20/1", new DateTime(2018, 1, 20, 7, 0, 0), text.ToString(), temp));
        }

        // Public Methods
        public void AddBlogPost(BlogPost bp)
        {
            blogPostCollection.Add(bp);
            NotifyPropertyChanged();
        }
    }
}
