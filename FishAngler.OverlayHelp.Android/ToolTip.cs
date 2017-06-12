using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using Android.Views.Animations;

namespace FishAngler.OverlayHelp.Android
{
    public class Tooltip
    {
        public Tooltip()
        {
            /* default values */
            Title = "";
            Description = "";
            BackgroundColor = Color.ParseColor("#07819e");
            TextColor = Color.ParseColor("#FFFFFF");

            EnterAnimation = new AlphaAnimation(0f, 1f);
            EnterAnimation.Duration = 1000;
            EnterAnimation.FillAfter = true;
            Shadow = true;
            Width = -1;

            Gravity = (int)GravityFlags.Center;
        }

		public String Title { get; set; }
		public String Description { get; set; }
		public Color BackgroundColor { get; set; }
        public Drawable Background { get; set; }
		public Color TextColor { get; set; }
		public bool Shadow { get; set; }
		public int Gravity { get; set; }
		public Animation EnterAnimation { get; set; }
		public View.IOnClickListener OnClickListener { get; set; }
		public int Width { get; set; }
        public int BorderRadius { get; set; }

		public Tooltip SetTitle(String title)
        {
            Title = title;
            return this;
        }

        public Tooltip SetDescription(String description)
        {
            Description = description;
            return this;
        }

        public Tooltip SetBackgroundColor(Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
            return this;
        }

        public Tooltip SetBackground(Drawable background)
		{
			Background = background;
			return this;
		}

		public Tooltip SetTextColor(Color textColor)
        {
            TextColor = textColor;
            return this;
        }

        public Tooltip SetEnterAnimation(Animation enterAnimation)
        {
            EnterAnimation = enterAnimation;
            return this;
        }

        public Tooltip SetGravity(int gravity)
        {
            Gravity = gravity;
            return this;
        }

        public Tooltip SetShadow(bool shadow)
        {
            Shadow = shadow;
            return this;
        }

        public Tooltip SetWidth(int px)
        {
            if (px >= 0) Width = px;
            return this;
        }

		public Tooltip SetBorderRadius(int radius)
		{
            BorderRadius = radius;
			return this;
		}

		public Tooltip SetOnClickListener(View.IOnClickListener onClickListener)
        {
            OnClickListener = onClickListener;
            return this;
        }
    }
}