using Microsoft.Azure.Mobile.Server;
using System;

namespace badmintonclubService.DataObjects
{
    public class BlogPost : EntityData
    {
        public DateTime DateTimePublished { get; set; }

        public int UserID { get; set; }

        public string BodyOfPost { get; set; }
        public string Title { get; set; }
    }
}