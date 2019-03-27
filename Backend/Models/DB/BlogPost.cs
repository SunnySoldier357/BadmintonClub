using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DB
{
    public class BlogPost
    {
        // Public Properties
        public DateTime DatePublished { get; set; }

        public Guid Id { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string Title { get; set; }

        public string DatePublishedString =>
            string.Format("Posted on {0}.", DatePublished.ToString().ToLower());

        [Required]
        public User Publisher { get; set; }

        // Constructors
        // TODO: Remove if not needed
        public BlogPost() : this(null, null, null, new DateTime()) { }

        public BlogPost(BlogPost post) :
            this(post.Title, post.Body, post.Publisher, post.DatePublished) { }

        public BlogPost(string title, string body, User publisher, DateTime datePublished)
        {
            Title = title;

            if (datePublished == new DateTime())
                DatePublished = DateTime.Now;
            else
                DatePublished = datePublished;

            Body = body;
            Publisher = publisher;
        }

        // Public Methods
        public void Update(BlogPost updated)
        {
            Body = updated.Body;
            Title = updated.Title;
        }
    }
}