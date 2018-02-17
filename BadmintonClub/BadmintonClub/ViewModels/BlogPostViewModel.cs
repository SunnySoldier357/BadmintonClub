using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BadmintonClub.ViewModels
{
    public class BlogPostViewModel : INotifyPropertyChanged
    {
        // Private Properties
        private BadmintonDAL bDAL;
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

            bDAL = new BadmintonDAL();
            foreach (DataRow row in bDAL.BlogPostsDT.Rows)
            {
                int userID = (int)row["userID"];
                User user = bDAL.GetUser(userID);
                DateTime datetime = (DateTime)row["datePublished"];
                blogPostCollection.Add(new BlogPost(row["title"].ToString(), datetime, row["bodyOfPost"].ToString(), user));
            }
        }

        // Public Methods
        public void AddBlogPost(BlogPost bp)
        {
            blogPostCollection.Add(bp);
            NotifyPropertyChanged();
        }
    }
}
