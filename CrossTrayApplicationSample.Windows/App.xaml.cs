using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using CrossTrayApplicationSample.Shared;
using Application = Xamarin.Forms.Application;

namespace CrossTrayApplicationSample.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    { 
        private NotifyIcon _notifyIcon;
        private bool _isExit;
        private static Shared.App application = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            Forms.Init();

            base.OnStartup(e);

            _notifyIcon = new NotifyIcon();
            _notifyIcon.MouseUp += NotifyIconOnMouseUp;
            _notifyIcon.MouseMove += NotifyIconOnMouseMove;
            _notifyIcon.Icon = CrossTrayApplicationSample.Windows.Properties.Resources.TrayIcon;
            _notifyIcon.Visible = true;
            application = new Shared.App();
            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
                new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", 
                CrossTrayApplicationSample.Windows.Properties.Resources.TrayIcon.ToBitmap()).Click += 
                (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            INotificationService notificationService = DependencyService.Get<INotificationService>();
            notificationService.ShowNotification("windows", "we are on wiwndows");

            /*_isExit = true;
            if (MainWindow != null)
            {
                MainWindow.Closing -= MainWindow_Closing;
                //MainWindow.LostFocus -= MainWindow_LostFocus;
                MainWindow.Close();
                MainWindow = null;
            }
            _notifyIcon.Dispose();
            _notifyIcon = null;

            // Stop the application
            Current.Shutdown();*/
        }

        /// <summary>
        /// Toggles the window between visible and hidden.
        /// </summary>
        private void ToggleWindow()
        { 
            // Create window when it is opened for the first time
            if (MainWindow == null)
            {
                MainWindow = new FormsApplicationPage
                {
                    Title = "Xamarin.Forms tray!",
                    Height = 600,
                    Width = 350,
                    Topmost = true,
                    ShowInTaskbar = false,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.ToolWindow
                };
                
                ((FormsApplicationPage)MainWindow).LoadApplication(application);
                MainWindow.Closing += MainWindow_Closing;
                Application.Current.SendStart();
            }

            // Hide the window when it is visible
            if (MainWindow.IsVisible)
            {
                // Hide!
                MainWindow.Deactivated -= MainWindowOnDeactivated;
                MainWindow.Hide();
                Application.Current.SendSleep();
            }
            // Show the window when it is not visible
            else
            {
                // Position window 
                MainWindow.Left = SystemParameters.WorkArea.Width - MainWindow.Width - 50;
                MainWindow.Top = SystemParameters.WorkArea.Height - MainWindow.Height - 50;
                // Show!
                MainWindow.Show();
                MainWindow.Activate();
                MainWindow.Deactivated += MainWindowOnDeactivated;
                Application.Current.SendResume();
            }
        }

        // Toggle the window on a left click on the icon
        private void NotifyIconOnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ToggleWindow();
        }

        // Toggle when the window 'X' was clicked
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                // Only hide the window to avoid recreating it when it should get displayed again
                ToggleWindow();
            }
        }

        private System.Drawing.Point? _lastMousePositionInIcon;

        // Store the current position of the mouse on the icon to check if the mouse clicked inside the icon
        // when the window gets deactivated to avoid a duplicated window toggle
        private void NotifyIconOnMouseMove(object sender, MouseEventArgs e)
        {
            _lastMousePositionInIcon = Control.MousePosition;
        }

        /// <summary>
        /// Called when clicked outside the window.
        /// Toggles the window to get hidden.
        /// </summary> 
        private void MainWindowOnDeactivated(object sender, EventArgs e)
        {
            // Check if the deactivation came by clicking the icon since this already toggles the window
            if (_lastMousePositionInIcon.HasValue && _lastMousePositionInIcon == Control.MousePosition)
                return;
            ToggleWindow();
        }
    }
}

