using Android.App;
using Android.Gms.Common;
using Android.Widget;
using Android.OS;
using Android.Util;
using Gcm.Client;

namespace NotificationHubSample.Droid
{
    [Activity(Label = "NotificationHubSample Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextView message;

        public static MainActivity instance;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            instance = this;

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            message = FindViewById<TextView>(Resource.Id.message);

            if (IsPlayServicesAvailable())
            {
                RegisterWithGCM();
            }

        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    message.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    message.Text = "Sorry, this device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                message.Text = "Google Play Services is available.";
                return true;
            }
        }

        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            Log.Info("MainActivity", "Registering...");
            GcmClient.Register(this, Constants.SenderID);
        }
    }
}

