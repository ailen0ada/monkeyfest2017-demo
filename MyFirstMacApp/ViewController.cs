using System;

using AppKit;
using Foundation;

namespace MyFirstMacApp
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void ShowGreeting(NSObject sender)
        {
            var msg = $"Hello, {this.Name}!";
            var alert = new NSAlert
            {
                MessageText = msg,
                InformativeText = "MonkeyFest 2017 demonstration",
                AlertStyle = NSAlertStyle.Informational
            };
            alert.RunSheetModal(this.View.Window);
        }

        NSString name;

        [Outlet]
        public NSString Name
        {
            get => name;

            set
            {
                this.WillChangeValue(nameof(Name));
                name = value;
                this.DidChangeValue(nameof(Name));
            }
        }
    }
}
