using BadmintonClub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BadmintonClub.ViewModels
{
    public class BlogPostViewModel : INotifyPropertyChanged
    {
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

        public BlogPostViewModel()
        {
            blogPostCollection = new ObservableCollection<BlogPost>();
            initialiseCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            NotifyPropertyChanged();
        }
    }
}
