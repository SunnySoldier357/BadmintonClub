using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace badmintonclubService.DataObjects
{
    public class Match : EntityData
    {
        public string PlayerID { get; set; }
        public int PlayerScore { get; set; }
        
        public string OpponentID { get; set; }
        public int OpponentScore { get; set; }
    }
}