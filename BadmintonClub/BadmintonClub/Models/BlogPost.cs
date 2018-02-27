﻿using System;

namespace BadmintonClub.Models
{
    public class BlogPost
    {
        // Public Properties
        public DateTime DateTimePublished { get; set; }

        public string BodyOfPost { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string UserID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string BodyOfPostJustified
        {
            get { return string.Format("<html><body  style=\"text-align: justify;\"><p>{0}</p></body></html>", BodyOfPost); }
        }
        [Newtonsoft.Json.JsonIgnore]
        public string DateTimePublishedString
        {
            get { return string.Format("Posted on {0} {1}.", DateTimePublished.ToShortDateString(), 
                DateTimePublished.ToShortTimeString().ToLower()); }
        } 

        [Newtonsoft.Json.JsonIgnore]
        public User Publisher { get; set; }

        // Constructors
        public BlogPost() : this("NIL", DateTime.Now, "NIL", new User()) {}

        public BlogPost(string title, DateTime dateTimePublished, string bodyOfPost, User user)
        {
            Title = title;
            DateTimePublished = dateTimePublished;
            BodyOfPost = bodyOfPost;
            Publisher = user;
        }

        // DEBUG PURPOSES
        public override string ToString()
        {
            return string.Format("ID: {0}, Title: {1}, UserID: {2}, User: [{3}]", Id, Title, UserID, Publisher.ToString());
        }
    }
}