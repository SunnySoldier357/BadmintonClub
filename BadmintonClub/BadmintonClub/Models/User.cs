using System;
using System.Collections.Generic;
using System.Text;

namespace BadmintonClub.Models
{
    public class User
    {
        // Static Properties
        public static Position Master = new Position("", 2);
        public static Position Admin = new Position("", 1);
        public static Position Member = new Position("", 0);

        // Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Position PositionInClub { get; set; }
    }
}
