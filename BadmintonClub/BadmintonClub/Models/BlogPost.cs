using System;
using System.Collections.Generic;
using System.Text;

namespace BadmintonClub.Models
{
    public class BlogPost
    {
        // Properties
        public string Title { get; set; }
        public DateTime DateTimePublished { get; set; }
        public string BodyOfPost { get; set; }
        public string PublisherName { get; set; }

        public BlogPost(string title, DateTime dateTimePublished, string bodyOfPost, string publisherName)
        {
            Title = title;
            DateTimePublished = dateTimePublished;
            BodyOfPost = bodyOfPost;
            PublisherName = publisherName;
        }
    }
}
