using Xamarin.Forms;

namespace CrossTrayIconSample.Shared
{
    public class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new MainPage());
        }
    }
}