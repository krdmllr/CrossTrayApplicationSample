using System.Diagnostics;
using Xamarin.Forms;

namespace CrossTrayApplicationSample.Shared
{
    public class App : Application
    {
        public static INotificationService NotificationService = null;
        public App(INotificationService notificationService)
        {
            MainPage = new NavigationPage(new MainPage());
            NotificationService = notificationService;
        }

        protected override void OnStart()
        {
            base.OnStart();
            Debug.WriteLine("Application started");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Debug.WriteLine("Application sleeps");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Debug.WriteLine("Application resumes");
        }
    }
}