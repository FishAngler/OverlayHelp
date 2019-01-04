// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace FishAngler.OverlayHelp.iOS.Example
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton button { get; set; }

        [Outlet]
        UIKit.UIView label1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton button1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton button2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton button3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton button4l { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton button4r { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (button1 != null) {
                button1.Dispose ();
                button1 = null;
            }

            if (button2 != null) {
                button2.Dispose ();
                button2 = null;
            }

            if (button3 != null) {
                button3.Dispose ();
                button3 = null;
            }

            if (button4l != null) {
                button4l.Dispose ();
                button4l = null;
            }

            if (button4r != null) {
                button4r.Dispose ();
                button4r = null;
            }

            if (label1 != null) {
                label1.Dispose ();
                label1 = null;
            }
        }
    }
}