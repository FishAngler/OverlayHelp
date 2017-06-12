using System;
using UIKit;

namespace FishAngler.OverlayHelp.iOS
{
    public class Overlay
    {
        public Overlay() : this(true, UIColor.FromRGBA(0, 0, 0, 0.7f))
		{
		}

		public bool DisableClick { get; set; }
		public UIColor BackgroundColor { get; set; }
		public bool DisableClickThroughHole { get; set; }
		public Action OnClickListener { get; set; }

		public Overlay(bool disableClick, UIColor backgroundColor)
		{
			DisableClick = disableClick;
			BackgroundColor = backgroundColor;
		}

		public Overlay SetBackgroundColor(UIColor backgroundColor)
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

		public Overlay SetOnClickListener(Action onClickListener)
		{
			OnClickListener = onClickListener;
			return this;
		}
	}
}

