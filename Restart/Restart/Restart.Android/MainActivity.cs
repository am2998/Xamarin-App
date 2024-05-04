using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using CarouselView.FormsPlugin.Droid;
using Plugin.LocalNotifications;
using Plugin.Fingerprint;
using Android;



namespace Restart.Droid
{
    [Activity(Label = "TrackMyMoney", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };


        protected override void OnCreate(Bundle savedInstanceState)
            {
           
            CrossFingerprint.SetCurrentActivityResolver (( ) => this);


            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Rg.Plugins.Popup.Popup.Init(this);

            base.OnCreate(savedInstanceState);

      

            Forms.SetFlags("SwipeView_Experimental");
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.nospese;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            CarouselViewRenderer.Init ( );
            Xamarin.FormsMaps.Init (this, savedInstanceState);
            LoadApplication (new App());
            }


        protected override void OnStart ( )
            {



            base.OnStart ( );

            

            if ((int)Build.VERSION.SdkInt >= 23)
                {
                if (CheckSelfPermission (Manifest.Permission.AccessFineLocation) != Permission.Granted)
                    {
                    RequestPermissions (LocationPermissions, RequestLocationId);
                    }
                else
                    {
                    // Permissions already granted - display a message.
                    }
                }

   
            }


        public override void OnRequestPermissionsResult (int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
            {
            if (requestCode == RequestLocationId)
                {
                if (( grantResults.Length == 1 ) && ( grantResults[0] == (int)Permission.Granted ))
                    {
                    // Permissions granted - display a message.
                    }
                else
                    {
                    // Permissions denied - display a message.
                    }
                }
            else
                {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult (requestCode, permissions, grantResults);

                base.OnRequestPermissionsResult (requestCode, permissions, grantResults);
                }
            }


        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }


        protected override void OnActivityResult (int requestCode, Result resultCode, Android.Content.Intent data)
            {
           //base.OnActivityResult (requestCode, resultCode, intent);
            base.OnActivityResult (requestCode, resultCode, data);

            }



        }
}