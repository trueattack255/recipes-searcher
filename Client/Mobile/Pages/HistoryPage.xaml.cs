using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
		public HistoryPage()
        {
            InitializeComponent();

            //NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}