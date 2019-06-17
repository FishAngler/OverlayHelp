using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V4.Content;
using System;
using Android.Graphics;

namespace FishAngler.OverlayHelp.Android.Example
{
    [Activity(Label = "OverlayHelp.Android.Example", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

			var density = Resources.DisplayMetrics.Density;

			Button button1 = FindViewById<Button>(Resource.Id.button1);
			var overlayHelp1 = new Help(this);
			var highlight1 = new Highlight()
			{
                HighlightStyle = Highlight.Style.RoundedRectangle
			};

			var tooltip1 = new Tooltip()
			{
				Description = "Rounded rect highlight over control with tooltip below",
				Shadow = true,
				Gravity = (int)GravityFlags.Bottom,
				Width = (int)(190 * density),
				Background = ContextCompat.GetDrawable(this, Resource.Drawable.rounded_corners),
			};

			overlayHelp1
				.AddControl(button1, tooltip1, highlight1)
				.Play();

			Button button2 = FindViewById<Button>(Resource.Id.button2);
			var overlayHelp2 = new Help(this);
			var highlight2 = new Highlight()
			{
				HighlightStyle = Highlight.Style.Rectangle
			};

			var tooltip2 = new Tooltip()
			{
				Description = "Rect highlight over custom rect target with right tooltip",
				Shadow = true,
				Gravity = (int)GravityFlags.Right,
				Width = (int)(120 * density),
				Background = ContextCompat.GetDrawable(this, Resource.Drawable.rounded_corners),
			};

            overlayHelp2
                .AddRect(new Rect((int)(5 * density), (int)(120 * density), (int)(60 * density), (int)(190 * density)), tooltip2, highlight2);

			Button button3 = FindViewById<Button>(Resource.Id.button3);
			var overlayHelp3 = new Help(this);

			var overlay = new Overlay();
			//overlay.SetOnClickListener(() => overlayHelp3.Hide());
			var highlight3 = new Highlight()
			{
				HoleWidth = (int)(120 * density),
				HighlightStyle = Highlight.Style.Circle
			};

			var tooltip3 = new Tooltip()
			{
				Description = "Circle highlight over control with top tooltip",
				Shadow = true,
				Gravity = (int)GravityFlags.Top,
				Width = (int)(190 * density),
				Background = ContextCompat.GetDrawable(this, Resource.Drawable.rounded_corners),
			};

			overlayHelp3 = new Help(this);
            overlayHelp3
                .SetOverlay(overlay)
                .AddControl(button3, tooltip3, highlight3);


			Button button4l = FindViewById<Button>(Resource.Id.button4l);
			Button button4r = FindViewById<Button>(Resource.Id.button4r);

            var overlayHelp4 = new Help(this);

			var highlight4 = new Highlight()
			{
				HighlightStyle = Highlight.Style.RoundedRectangle
			};

			var tooltip4l = new Tooltip()
			{
				Description = "Double highlight over control with tooltip top",
				Shadow = true,
				Gravity = (int)GravityFlags.Top,
				Width = (int)(150 * density),
				Background = ContextCompat.GetDrawable(this, Resource.Drawable.rounded_corners),
			};

			var tooltip4r = new Tooltip()
			{
				Description = "Double highlight over control with tooltip bottom",
				Shadow = true,
				Gravity = (int)GravityFlags.Bottom,
				Width = (int)(150 * density),
				Background = ContextCompat.GetDrawable(this, Resource.Drawable.rounded_corners),
			};

			overlayHelp4 = new Help(this);
            overlayHelp4
                .AddControl(button4l, tooltip4l, highlight4)
                .AddControl(button4r, tooltip4r, highlight4);

			button1.Click += delegate 
            {
                overlayHelp1.Hide();
                overlayHelp2.Play();
            };
			
            button2.Click += delegate 
            {
				overlayHelp2.Hide();
                overlayHelp3.Play();
			};

			button3.Click += delegate 
            {
				overlayHelp3.Hide();
                overlayHelp4.Play();
            };

			button4l.Click += delegate 
            {
				overlayHelp4.Hide();
			};

			button4r.Click += delegate 
            {
				overlayHelp4.Hide();
			};
        }
    }
}

