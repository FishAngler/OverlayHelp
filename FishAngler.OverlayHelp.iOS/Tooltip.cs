using System;
using UIKit;

namespace FishAngler.OverlayHelp.iOS
{
	public enum GravityFlags
	{
		Bottom = 80,
        Left = 3,
        Right = 5,
        Top = 48,
	}

    public class Tooltip
	{
		public Tooltip()
		{
			/* default values */
			Title = "";
			Description = "";
            BackgroundColor = UIColor.FromRGB(7, 129, 158);
            TextColor = UIColor.White;

			UseEnterAnimation = true;
			Shadow = true;
			Width = -1;

            Gravity = (int)GravityFlags.Top;
		}

		public String Title { get; set; }
		public String Description { get; set; }
		public UIColor BackgroundColor { get; set; }
		public UIColor TextColor { get; set; }
		public bool Shadow { get; set; }
		public int Gravity { get; set; }
		public bool UseEnterAnimation { get; set; }
        public Action OnClickListener { get; set; }
		public UIView CustomView { get; set; }
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

		public Tooltip SetBackgroundColor(UIColor backgroundColor)
		{
			BackgroundColor = backgroundColor;
			return this;
		}

		public Tooltip SetTextColor(UIColor textColor)
		{
			TextColor = textColor;
			return this;
		}

		public Tooltip SetEnterAnimation(bool useEnterAnimation)
		{
			UseEnterAnimation = useEnterAnimation;
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

		public Tooltip SetOnClickListener(Action onClickListener)
		{
			OnClickListener = onClickListener;
			return this;
		}

		public UIView GetCustomView()
		{
			return CustomView;
		}

		public Tooltip SetCustomView(UIView view)
		{
			CustomView = view;
			return this;
		}
	}
}