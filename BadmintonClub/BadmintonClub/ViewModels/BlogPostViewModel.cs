using BadmintonClub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BadmintonClub.ViewModels
{
    public class BlogPostViewModel
    {
        /*
        public ObservableCollection<BlogPost> BlogPostCollection
        {
            get
            {
                ObservableCollection<BlogPost> sorted = (ObservableCollection<BlogPost>)(from g in BlogPostCollection orderby g.DateTimePublished descending select g);
                return sorted;
            }
            set { BlogPostCollection = value; }
        }
        */
        public ObservableCollection<BlogPost> BlogPostCollection { get; set; }

        public BlogPostViewModel()
        {
            BlogPostCollection = new ObservableCollection<BlogPost>();
            initialiseCollection();
        }

        private void initialiseCollection()
        {
            AddBlogPost(new BlogPost("Update 7/2", new DateTime(2018, 2, 7, 7, 55, 0), "This is a test blog", "Sandeep Singh Sidhu"));
            AddBlogPost(new BlogPost("Update 3/2", new DateTime(2018, 2, 3, 7, 55, 0), "This is a test blog", "Sandeep Singh Sidhu"));
            AddBlogPost(new BlogPost("Update 10/2", new DateTime(2018, 2, 10, 7, 55, 0), "This is a test blog", "Sandeep Singh Sidhu"));
        }

        public void AddBlogPost(BlogPost bp)
        {
            BlogPostCollection.Add(bp);
        }
    }
}
