using BadmintonClub.Models;
using BadmintonClub.Models.Data_Access_Layer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        // Private Properties
        private User signedInUser;

        // Constructor
		public ProfilePage()
		{
			InitializeComponent();
            BindingContext = signedInUser = (Application.Current as App).SignedInUser;

            // Padding for iOS to not cover status bar
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Refresh",
                Command = new Command(async () => 
                {
                    AzureTransaction azureTransaction = new AzureTransaction(new Transaction(null, TransactionType.SyncUserMatches));
                    await azureTransaction.ExecuteAsync();
                }),
                Icon = "refresh.png"
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Log Out",
                Command = new Command(() => (Application.Current as App).RestartApp()),
                Icon = "contacts.png"
            });
        }
    }
}