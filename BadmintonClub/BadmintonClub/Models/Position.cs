using System;
using System.Collections.Generic;
using System.Text;

namespace BadmintonClub.Models
{
    public class Position
    {
        private int priority;

        public string Title { get; set; }
        public int Priority { get { return priority; }
            set { priority = value == 0 || value == 1 || value == 2 ? value : 0; } } // 0-Member 1-Admin/Board 2-Master

        public Position(string title, int priority)
        {
            Title = title;
            Priority = priority;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position)
            {
                return Priority == ((Position) obj).Priority;
            }
            else if (obj is User)
            {
                User temp = (User) obj;
                return Priority == temp.PositionInClub.Priority;
            }
            else
                return false;
        }
    }
}
