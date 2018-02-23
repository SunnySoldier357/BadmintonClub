using System;

namespace BadmintonClub.Models
{
    public class BlogPost
    {
        // Public Properties
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime DateTimePublished { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public string DateTimePublishedString {
            get { return string.Format("Posted on {0} {1}.", DateTimePublished.ToShortDateString(), 
                DateTimePublished.ToShortTimeString().ToLower()); } } 

        public string BodyOfPost { get; set; }
        public int UserID { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public User User { get; set; }

        // Constructors
        public BlogPost() : this("Defalut", DateTime.Now, "NA", new User()) { }

        public BlogPost(string title, DateTime dateTimePublished, string bodyOfPost, User user)
        {
            Title = title;
            DateTimePublished = dateTimePublished;
            BodyOfPost = bodyOfPost;
            User = user;
        }
    }
}
