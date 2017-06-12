using System;
using Android.Graphics;
using Android.Views;

namespace FishAngler.OverlayHelp.Android
{
    public class HelpItem
    {
        View _targetView;
        Rect _targetRect;

        public HelpItem(View targetView)
        {
            _targetView = targetView;
        }

		public HelpItem(Rect targetRect)
		{
			_targetRect = targetRect;
		}

		public Rect TargetRect 
        { 
            get 
            {
                if (_targetRect == null && _targetView != null)
                {
					int[] pos = new int[2];
					_targetView.GetLocationOnScreen(pos);
					var rect = new Rect(pos[0], pos[1], pos[0] + _targetView.Width, pos[1] + _targetView.Height);
                    if (!rect.IsEmpty)
                    {
                        _targetRect = rect;
                    }
				}
                return _targetRect;
            }
        }
		public Tooltip Tooltip { get; set; }
		public Highlight Highlight { get; set; }
	}
}
