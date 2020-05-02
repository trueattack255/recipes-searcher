using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{
		public SearchPage()
		{
            InitializeComponent();

            //NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}