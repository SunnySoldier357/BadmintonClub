using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using System;
using System.Collections.Generic;
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
        private BadmintonDAL bDAL;
        private ObservableCollection<BlogPost> blogPostCollection;

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

        public event PropertyChangedEventHandler PropertyChanged;

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

            //initialiseCollection();
        }

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void initialiseCollection()
        {
            User user = new User("Sandeep", "Singh Sidhu", "Co-President", 2);
            AddBlogPost(new BlogPost("Update 7/2", new DateTime(2018, 2, 7, 7, 55, 0), "This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.", user));
            AddBlogPost(new BlogPost("Update 3/2", new DateTime(2018, 2, 3, 7, 55, 0), "This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.", user));
            AddBlogPost(new BlogPost("Update 10/2", new DateTime(2018, 2, 10, 7, 55, 0), "This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.This is a test blog.", user));
        }

        public void AddBlogPost(BlogPost bp)
        {
            blogPostCollection.Add(bp);
        }
    }
}
