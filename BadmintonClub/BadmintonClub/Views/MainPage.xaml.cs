using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadmintonClub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)] 
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			InitializeComponent();

            masterPage.ContentPane.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
                MasterBehavior = MasterBehavior.Popover;
		}

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.ContentPane.SelectedItem = null;
                IsPresented = false;
            }
        }
	}
}