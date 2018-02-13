using BadmintonClub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BadmintonClub.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public static User SignedInUser = new User();

        private ObservableCollection<User> userCollection;
        public ObservableCollection<User> UserCollection {
            get { return userCollection; }
            set { userCollection = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public UserViewModel()
        {
            userCollection = new ObservableCollection<User>();
            initialiseCollection();
        }

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void initialiseCollection()
        {
            userCollection.Add(new User("Sandeep", "Singh Sidhu", "Co-President", 2));
            userCollection.Add(new User("Marques", "Wong", "Co-President", 2));
            userCollection.Add(new User("Archit", "Guniligari", "Vice President", 1));
        }
    }
}
