using System;

namespace CrossTrayApplicationSample.Shared
{
    public interface INotificationService
    {
        void ShowNotification(string title, string message);
    }
}