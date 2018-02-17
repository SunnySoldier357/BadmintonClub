using BadmintonClub.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BadmintonClub.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        // Public Static Properties
        public static User SignedInUser = new User();

        // Private Properties
        private ObservableCollection<User> userCollection;

        // Public Properties
        public ObservableCollection<User> UserCollection {
            get { return userCollection; }
            set { userCollection = value; }
        }

        // Events and Event Handlers
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructors
        public UserViewModel()
        {
            userCollection = new ObservableCollection<User>();
            initialiseCollection();
        }

        // TO DO: NEED TO BE REPLACED TO CONNECT WITH THE DATABASE
        private void initialiseCollection()
        {
            userCollection.Add(new User("Sandeep", "Singh Sidhu", "Co-President", 2));
            userCollection.Add(new User("Marques", "Wong", "Co-President", 2));
            userCollection.Add(new User("Archit", "Guniligari", "Vice President", 1));
        }
    }
}
