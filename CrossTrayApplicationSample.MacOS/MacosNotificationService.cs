using CrossTrayApplicationSample.Shared;
using Foundation;

namespace CrossTrayApplicationSample.MacOS
{
    public class MacosNotificationService : INotificationService
    {
        public void ShowNotification(string title, string message)
        {
            // Trigger a local notification after the time has elapsed
            var notification = new NSUserNotification();

            // Add text and sound to the notification
            notification.Title = title;
            notification.InformativeText = message;
            notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
            notification.HasActionButton = true;
            NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);
        }
    }
}