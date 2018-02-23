using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace badmintonclubService.DataObjects
{
    public class BlogPost : EntityData
    {
        public string Title { get; set; }

        public DateTime DateTimePublished { get; set; }

        public string BodyOfPost { get; set; }

        public int UserID { get; set; }
    }
}