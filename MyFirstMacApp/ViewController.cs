using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace MyFirstMacApp
{
    public partial class ViewController : NSViewController, INSTableViewDataSource, INSTableViewDelegate
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            Friend = new Person { Honorific = Honorifics[0] };
            HistoryTableView.DataSource = this;
            HistoryTableView.Delegate = this;
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
            histories.Add(new Person { Name = friend.Name, Honorific = friend.Honorific });
            HistoryTableView.ReloadData();

            var msg = $"Hello, {this.Friend.Name}-{this.Friend.Honorific}!";
            var alert = new NSAlert
            {
                MessageText = msg,
                InformativeText = "MonkeyFest 2017 demonstration",
                AlertStyle = NSAlertStyle.Informational
            };
            alert.RunSheetModal(this.View.Window);
        }

        Person friend;

        [Outlet]
        public Person Friend
        {
            get
            {
                return friend;
            }

            set
            {
                WillChangeValue(nameof(Friend));
                friend = value;
                DidChangeValue(nameof(Friend));
            }
        }

        [Outlet]
        public NSString[] Honorifics { get; } = new[] { (NSString)"san", (NSString)"kun", (NSString)"chan", (NSString)"sama" };

        private List<Person> histories = new List<Person>();

        [Export("numberOfRowsInTableView:")]
        public nint GetRowCount(NSTableView tableView)
        {
            return histories.Count;
        }

        [Export("tableView:viewForTableColumn:row:")]
        public NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            var item = histories[(int)row];
            var text = item.ValueForKey((NSString)tableColumn.Identifier) ?? NSString.Empty;

            var view = (NSTableCellView)tableView.MakeView(tableColumn.Identifier, this);
            view.TextField.ObjectValue = text;
            return view;
        }

        [Export("tableViewSelectionDidChange:")]
        public void SelectionDidChange(NSNotification notification)
        {
            var row = HistoryTableView.SelectedRow;
            if (row < 0) return;

            var item = histories[(int)row];
            friend.Name = item.Name;
            friend.Honorific = item.Honorific;
        }
    }

    [Register(nameof(Person))]
    public class Person : NSObject
    {
        NSString name;

        [Outlet]
        public NSString Name
        {
            get => name;

            set
            {
                this.WillChangeValue(nameof(Name));
                this.WillChangeValue(nameof(NameLength));
                name = value;
                this.DidChangeValue(nameof(Name));
                this.DidChangeValue(nameof(NameLength));
            }
        }

        [Outlet]
        public NSNumber NameLength => NSNumber.FromNInt(name?.Length ?? 0);

        NSString honorific;

        [Outlet]
        public NSString Honorific
        {
            get
            {
                return honorific;
            }

            set
            {
                this.WillChangeValue(nameof(Honorific));
                honorific = value;
                this.DidChangeValue(nameof(Honorific));
            }
        }
    }
}
