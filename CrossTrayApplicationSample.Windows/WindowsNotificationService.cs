using CrossTrayApplicationSample.Shared;
using Notifications.Wpf;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrossTrayApplicationSample.Windows.WindowsNotificationService))]
namespace CrossTrayApplicationSample.Windows
{
    public class WindowsNotificationService : INotificationService
    {
        public void ShowNotification(string title, string message)
        {
            var notificationManager = new NotificationManager();

            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Information
            });
        }
    }
}
