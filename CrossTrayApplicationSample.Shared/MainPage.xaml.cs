using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossTrayApplicationSample.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void GotoCalculatorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CalculatorPage());
        }

        private void ThrowNotification(object sender, EventArgs e)
        {
            INotificationService notificationService = DependencyService.Get<INotificationService>();
            notificationService.ShowNotification("Testtitle", "testmessage");
        }
    }
}