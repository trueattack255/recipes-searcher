using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V7.App;

namespace Mobile.Droid
{
    [Activity(Theme = "@style/SplashScreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        public override void OnBackPressed() { }

        async void SimulateStartup ()
        {
            await Task.Delay(2000);
            StartActivity(new Intent(Application.Context, typeof (MainActivity)));
        }
    }
}