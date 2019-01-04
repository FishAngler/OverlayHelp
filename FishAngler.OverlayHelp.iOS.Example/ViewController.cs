using System;
using CoreGraphics;
using UIKit;

namespace FishAngler.OverlayHelp.iOS.Example
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
			var overlayHelp1 = new Help(View);
			var highlight1 = new Highlight()
			{
				HighlightStyle = Highlight.Style.RoundedRectangle,
                HolePadding = 10
			};

			var tooltip1 = new Tooltip()
			{
				Description = "Rounded rect highlight over control with tooltip below",
				Shadow = true,
				Gravity = (int)GravityFlags.Bottom,
				Width = 190
			};

			overlayHelp1
				.AddControl(button1, tooltip1, highlight1)
				.Play();

			var overlayHelp2 = new Help(View);
			var highlight2 = new Highlight()
			{
				HighlightStyle = Highlight.Style.Rectangle
			};

			var tooltip2 = new Tooltip()
			{
				Description = "Rect highlight over custom rect target with right tooltip",
				Shadow = true,
				Gravity = (int)GravityFlags.Right,
				Width = 120
			};

			overlayHelp2
				.AddRect(new CGRect(5, 100, 50, 60), tooltip2, highlight2);

			var overlayHelp3 = new Help(View);

			var overlay = new Overlay();
			overlay.SetOnClickListener(() => overlayHelp3.Hide());
			var highlight3 = new Highlight()
			{
				HoleWidth = 100,
				HighlightStyle = Highlight.Style.Circle
			};

			var tooltip3 = new Tooltip()
			{
				Description = "Circle highlight over control with top tooltip",
				Shadow = true,
				Gravity = (int)GravityFlags.Top,
				Width = 190
			};

			overlayHelp3 = new Help(View);
			overlayHelp3
				.SetOverlay(overlay)
				.AddControl(button3, tooltip3, highlight3);

			var overlayHelp4 = new Help(View);

			var highlight4 = new Highlight()
			{
				HighlightStyle = Highlight.Style.RoundedRectangle,
                HolePadding = 10                                          
			};

			var tooltip4l = new Tooltip()
			{
				Description = "Double highlight over control with tooltip top",
				Shadow = true,
				Gravity = (int)GravityFlags.Top,
				Width = 150
			};

			var tooltip4r = new Tooltip()
			{
				Description = "Double highlight over control with tooltip bottom",
				Shadow = true,
				Gravity = (int)GravityFlags.Bottom,
				Width = 150,
			};

			overlayHelp4 = new Help(View);
			overlayHelp4
				.AddControl(button4l, tooltip4l, highlight4)
				.AddControl(button4r, tooltip4r, highlight4);


			var tooltip5 = new Tooltip()
			{
				Description = "Circle highlight over control with top tooltip",
				Shadow = true,
				Gravity = (int)GravityFlags.Top,
				Width = 190
			};

   //         label1.Hidden = true;

   //         // TODO: overlay click not working
			////var overlayHelp5 = new Help(View);
   ////         var overlay5 = new Overlay() { DisableClick = false };
   ////         overlay5.SetOnClickListener(() => overlayHelp5.Hide());

			////overlayHelp5
			//	//.SetOverlay(overlay)
   //             //.AddControl(label1, tooltip5);

			button1.TouchUpInside += delegate
			{
				overlayHelp1.Hide();
				overlayHelp2.Play();
			};

			button2.TouchUpInside += delegate
			{
				overlayHelp2.Hide();
				overlayHelp3.Play();
			};

			button3.TouchUpInside += delegate
			{
				overlayHelp3.Hide();
				overlayHelp4.Play();
			};

			button4l.TouchUpInside += delegate
			{
				overlayHelp4.Hide();
			};

			button4r.TouchUpInside += delegate
			{
				overlayHelp4.Hide();
			};
		}

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
