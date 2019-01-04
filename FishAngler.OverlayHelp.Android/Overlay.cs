using System;
using Android.Graphics;
using Android.Views;

namespace FishAngler.OverlayHelp.Android
{
    public class Overlay
    {
        public Overlay() : this(true, Color.ParseColor("#aa000000"))
		{			
		}

		public bool DisableClick { get; set; }
		public Color BackgroundColor { get; set; }
		public bool DisableClickThroughHole { get; set; }
		public Action OnClickAction { get; set; }

		public Overlay(bool disableClick, Color backgroundColor)
		{
			DisableClick = disableClick;
			BackgroundColor = backgroundColor;
		}

		public Overlay SetBackgroundColor(Color backgroundColor)
		{
			BackgroundColor = backgroundColor;
			return this;
		}

		public Overlay disableClick(bool yesNo)
		{
			DisableClick = yesNo;
			return this;
		}

        public Overlay disableClickThroughHole(bool yesNo)
		{
			DisableClickThroughHole = yesNo;
			return this;
		}

		public Overlay SetOnClickAction(Action onClickAction)
		{
			OnClickAction = onClickAction;
			return this;
		}
	}
}
