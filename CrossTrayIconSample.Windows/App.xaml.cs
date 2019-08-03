using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CrossTrayIconSample.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Extensions;
using Point = Xamarin.Forms.Point;

namespace CrossTrayIconSample.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private enum TaskBarPosition { Left, Top, Right, Bottom }

        private NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            Forms.Init();

            base.OnStartup(e);

            _notifyIcon = new NotifyIcon();
            _notifyIcon.MouseUp += (s, args) =>
            {
                if (args.Button == MouseButtons.Left)
                    ToggleWindow();
            };
            _notifyIcon.Icon = CrossTrayIconSample.Windows.Properties.Resources.TrayIcon;
            _notifyIcon.Visible = true;

            CreateContextMenu();
        }

        /// <summary>
        /// Toggles the window between visible and hidden.
        /// </summary>
        private void ToggleWindow()
        {
            // Create window when it is opened for the first time
            if (MainWindow == null)
            {
                MainWindow = new MainWindow
                {
                    Content = new TrayPage().ToFrameworkElement()
                };
                MainWindow.Closing += MainWindow_Closing;
            }

            // Hide the window when it is visible
            if (MainWindow.IsVisible)
            {
                // Hide!
                MainWindow.Hide();
            }
            // Show the window when it is not visible
            else
            {
                // Position window
                Point topLeftPosition = GetPosition(); 
                MainWindow.Left = topLeftPosition.X;
                MainWindow.Top = topLeftPosition.Y;

                // Show!
                MainWindow.Show();
            }
        }

        /// <summary>
        /// Get the top left point of the view based on the cursor and task bar position.
        /// </summary>
        private Point GetPosition()
        {
            // The current cursor position (when the icon is pressed)
            var cursor = Control.MousePosition;
            // Additional offset from the taskbar
            var offset = 10;
            // The work area without the task bar where the view is visible
            var desktopWorkingArea = SystemParameters.WorkArea; 
            // The complete screen size
            var screenBounds = Screen.PrimaryScreen.Bounds;

            // Get the task bar position
            var taskBarOrientation = TaskBarPosition.Bottom;
            if (desktopWorkingArea.Left > 0)
                taskBarOrientation = TaskBarPosition.Left;
            if (desktopWorkingArea.Right < screenBounds.Right)
                taskBarOrientation = TaskBarPosition.Right;
            if (desktopWorkingArea.Top > 0)
                taskBarOrientation = TaskBarPosition.Top; 

            switch (taskBarOrientation)
            {
                case TaskBarPosition.Left:
                    // Window is in the lower left corner
                    return new Point(desktopWorkingArea.Left + offset, cursor.Y - MainWindow.Height); 
                case TaskBarPosition.Top:
                    // Window is in the top right corner
                    return new Point(cursor.X - MainWindow.Width, desktopWorkingArea.Top + offset); 
                case TaskBarPosition.Right:
                    // Window is in the lower right corner
                    return new Point(desktopWorkingArea.Right - MainWindow.Width - offset, cursor.Y - MainWindow.Height);
                case TaskBarPosition.Bottom:
                    // Window is in the lower right corner
                    return new Point(cursor.X - MainWindow.Width, desktopWorkingArea.Bottom - MainWindow.Height - offset);
                default:
                    return Point.Zero;
            }
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
                new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", CrossTrayIconSample.Windows.Properties.Resources.TrayIcon.ToBitmap()).Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            if (MainWindow != null)
            {
                MainWindow.Closing -= MainWindow_Closing;
                MainWindow.Close();
            }
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }


        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                // Only hide the window to avoid recreating it when it should get displayed again
                MainWindow.Hide();
            }
        }
    }
}

