using Microsoft.Azure.Mobile.Server;
using System;

namespace badmintonclubService.DataObjects
{
    public class BlogPost : EntityData
    {
        public string Title { get; set; }

        public DateTime DateTimePublished { get; set; }

        public string BodyOfPost { get; set; }
        public string UserID { get; set; }
    }
}