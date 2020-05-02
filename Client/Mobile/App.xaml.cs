using FreshMvvm;
using Mobile.Interfaces;
using Mobile.PageModels;
using Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            FreshIOC.Container.Register<IWebService, WebService>();
            FreshIOC.Container.Register<ISQLiteService, SQLiteService>();

            var mainPage = FreshPageModelResolver.ResolvePageModel<SearchPageModel>();

            var basicNavContainer = new FreshNavigationContainer(mainPage);
            basicNavContainer.BackgroundColor = (Color) Resources["darkColor"];
            basicNavContainer.BarTextColor = (Color)Resources["mainColor"];

            MainPage = basicNavContainer;
        }
    }
}
