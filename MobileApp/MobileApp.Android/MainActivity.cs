using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

namespace MobileApp.Droid
{
    [Activity(Label = "MobileApp", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);

            CurrentPlatform.Init();
            LoadApplication(new App());
        }
    }
}