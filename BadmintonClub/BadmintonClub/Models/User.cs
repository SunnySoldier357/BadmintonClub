using System;
using System.Collections.Generic;
using System.Text;

namespace BadmintonClub.Models
{
    public class User
    {
        private int clearanceLevel;

        // Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Title { get; set; }
        public int ClearanceLevel { get { return clearanceLevel; }
            set { clearanceLevel = (value == 0 || value == 1 | value == 2) ? value : 0; } } // 0-Member 1-Admin/Board 2-Master

        public User(string firstName, string lastName, string title, int clearanceLevel)
        {
            FirstName = firstName;
            LastName = lastName;
            Title = title + (title.ToLower().Contains("of") ? " for" : " of") + " Badminton Club";
            ClearanceLevel = clearanceLevel;
        }

        public bool IsMaster()
        {
            return ClearanceLevel >= 2;
        }

        public bool IsAdmin()
        {
            return ClearanceLevel >= 1;
        }
    }
}
