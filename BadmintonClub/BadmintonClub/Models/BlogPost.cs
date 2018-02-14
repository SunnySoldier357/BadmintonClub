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
        public string DateTimePublishedString {
            get { return string.Format("Posted on {0} {1}.", DateTimePublished.ToShortDateString(), 
                DateTimePublished.ToShortTimeString().ToLower()); } } 

        public string BodyOfPost { get; set; }
        public User User { get; set; }

        public BlogPost(string title, DateTime dateTimePublished, string bodyOfPost, User user)
        {
            Title = title;
            DateTimePublished = dateTimePublished;
            BodyOfPost = bodyOfPost;
            User = user;
        }
    }
}
