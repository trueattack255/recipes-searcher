using System;
using System.Net.Http;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using FFImageLoading.Config;
using Xamarin.Forms;

namespace Mobile.Droid
{
    [Activity(Label = "QuickCook", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly int REQUEST_CAMERA = 256;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Forms.Init(this, bundle);

            var config = new Configuration
            {
                HttpClient = new HttpClient(new ModernHttpClient.NativeMessageHandler()),
                FadeAnimationDuration = 250,
            };

            FFImageLoading.ImageService.Instance.Initialize(config);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            if (int.Parse(Build.VERSION.Sdk) >= 23)
            {
                var permissionState = ContextCompat.CheckSelfPermission(this,
                    "android.permission.CAMERA");

                if (permissionState != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, REQUEST_CAMERA);
                }
            }

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_CAMERA)
                ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}