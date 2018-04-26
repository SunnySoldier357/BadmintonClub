using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
        // Public Properties
        public ListView ContentPane { get { return contentPane; } }

        // Constructor
        public MasterPage()
		{
			InitializeComponent();
		}
	}
}