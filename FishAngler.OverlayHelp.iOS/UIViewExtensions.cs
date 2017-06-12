using System;
using CoreGraphics;
using UIKit;

namespace FishAngler.OverlayHelp.iOS
{
    public static class UIViewExtensions
    {
        public static void Fade(this UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
		{
			var minAlpha = (nfloat)0.0f;
			var maxAlpha = (nfloat)1.0f;

			view.Alpha = isIn ? minAlpha : maxAlpha;
			view.Transform = CGAffineTransform.MakeIdentity();
			UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
				() =>
				{
					view.Alpha = isIn ? maxAlpha : minAlpha;
				},
				onFinished
			);
		}
    }
}
