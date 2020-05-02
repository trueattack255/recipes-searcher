using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePage : CarouselPage
    {   
        public RecipePage()
        {
            InitializeComponent();

            //NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}