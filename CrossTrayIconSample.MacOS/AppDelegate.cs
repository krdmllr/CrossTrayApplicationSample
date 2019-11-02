using System;
using AppKit;
using CrossTrayIconSample.Shared;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace CrossTrayIconSample.MacOS
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private NSStatusItem _statusBarItem;
        private NSMenu _menu;
        private NSViewController _mainPage;

        public AppDelegate()
        {
            Forms.Init();
            CreateStatusItem();
            Application.SetCurrentApplication(new App());
        }        

        private void CreateStatusItem()
        {
            NSStatusBar statusBar = NSStatusBar.SystemStatusBar;
            _statusBarItem = statusBar.CreateStatusItem(NSStatusItemLength.Variable);
            _statusBarItem.Button.Image = NSImage.ImageNamed("TrayIcon.ico");

            _statusBarItem.SendActionOn(NSTouchPhase.Any);
            _statusBarItem.Action = new ObjCRuntime.Selector("MenuAction:");

            _menu = new NSMenu();

            var closeAppItem = new NSMenuItem("Close");
            closeAppItem.Activated += CloseAppItem_Activated;
            _menu.AddItem(closeAppItem);
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        private void CloseAppItem_Activated(object sender, EventArgs e)
        {
            NSApplication.SharedApplication.Terminate(this);
        }

        [Export("MenuAction:")]
        public void ButtonClickAction(NSStatusItem item)
        {
            var currentEvent = NSApplication.SharedApplication.CurrentEvent;
            switch (currentEvent.Type)
            {
                case NSEventType.LeftMouseUp:
                    ShowWindow();
                    break;
                case NSEventType.RightMouseUp:
                    _statusBarItem.PopUpStatusItemMenu(_menu);
                    break;
            }
        }

        private void ShowWindow()
        { 
            if(_mainPage == null)
            {
                // If you dont need a navigation bar, just use this line
                //_mainPage = Application.Current.MainPage.CreateViewController();

                // Create a container view which shows the navigation bar
                var storyboard = NSStoryboard.FromName("Main", null);
                var controller = storyboard.InstantiateControllerWithIdentifier("Container") as Container; 
                _mainPage = controller;
                controller.SetContent(Application.Current.MainPage.CreateViewController()); 
                _mainPage.View.Frame = new CoreGraphics.CGRect(0, 0, 400, 700);

                Application.Current.SendStart();
            }
            else
            {
                Application.Current.SendResume();
            }

            var popover = new NSPopover
            {
                ContentViewController = _mainPage,
                Behavior = NSPopoverBehavior.Transient,
                Delegate = new PopoverDelegate()
            };
            popover.Show(_statusBarItem.Button.Bounds, _statusBarItem.Button, NSRectEdge.MaxYEdge);
        }

        class PopoverDelegate : NSPopoverDelegate
        {
            public override void DidClose(NSNotification notification)
            { 
                Application.Current.SendSleep();
            }
        }
    }
}
