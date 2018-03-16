using MvvmHelpers;
namespace BadmintonClub.Models
{
    public class ObservableObject<T> : ObservableObject
    {
        // Private Properties
        private T _obj;

        // Public Properties
        public T Object
        {
            get => _obj;
            set
            {
                SetProperty(ref _obj, value);
                OnPropertyChanged("_obj");
                OnPropertyChanged("Object");
            }
        }
    }
}
