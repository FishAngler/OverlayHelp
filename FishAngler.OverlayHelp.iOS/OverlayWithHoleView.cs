﻿using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace FishAngler.OverlayHelp.iOS
{
    public class OverlayWithHoleView : UIView
    {
        const int DEFAULT_CIRCLE_WIDTH = 50;
        IList<HelpItem> _helpItems; // This is the list of views to be highlighted, where the holes should be placed
		Overlay _overlay;

        public OverlayWithHoleView(IList<HelpItem> helpItems, Overlay overlay)
        {
            _helpItems = helpItems;
            _overlay = overlay;
            BackgroundColor = overlay.BackgroundColor;
            UserInteractionEnabled = true;
        }

        public event Action OverlayClick;

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            var res = _overlay.DisableClickThroughHole || !_helpItems.Select(hi => hi.TargetRect).Any(t => t.Contains(point));
            return res;
        }

        public void CleanUp()
        {
			if (Superview != null)
			{
				RemoveFromSuperview();
			}
		}

        public IList<CGRect> CalculatedHoleRects
        {
            get 
            {
                var calculatedHoleRects = new List<CGRect>();
                foreach(var helpItem in _helpItems)
                {
                    if (helpItem.Highlight != null)
                    {
						var rect = new CGRect(
							CalculateLeft(helpItem.TargetRect, helpItem.Highlight),
							CalculateTop(helpItem.TargetRect, helpItem.Highlight),
							CalculateWidth(helpItem.TargetRect, helpItem.Highlight),
							CalculateHeight(helpItem.TargetRect, helpItem.Highlight));
						    calculatedHoleRects.Add(rect);

					}
                    else 
                    {
						calculatedHoleRects.Add(CGRect.Empty);
					}
                }
                return calculatedHoleRects;
            }
        }

        nfloat CalculateLeft(CGRect viewHoleTarget, Highlight highlight)
		{
			nfloat calculatedLeft;
			if (highlight.HighlightStyle == Highlight.Style.Circle)
			{
				calculatedLeft = highlight.HoleWidth != Highlight.NOT_SET
										 ? viewHoleTarget.Left + viewHoleTarget.Width / 2 - highlight.HoleWidth / 2
										 : viewHoleTarget.Left + viewHoleTarget.Width / 2 - DEFAULT_CIRCLE_WIDTH / 2;
			}
			else
			{
				calculatedLeft = highlight.HoleWidth != Highlight.NOT_SET
										 ? viewHoleTarget.Left + viewHoleTarget.Width / 2 - highlight.HoleWidth / 2 - highlight.HolePadding
										 : viewHoleTarget.Left - highlight.HolePadding;
			}

			return calculatedLeft;
		}

		nfloat CalculateTop(CGRect viewHoleTarget, Highlight highlight)
		{
			nfloat calculatedTop;
			if (highlight.HighlightStyle == Highlight.Style.Circle)
			{
				calculatedTop = highlight.HoleWidth != Highlight.NOT_SET
										? viewHoleTarget.Top + viewHoleTarget.Height / 2 - highlight.HoleWidth / 2 - highlight.HolePadding
										: viewHoleTarget.Top + viewHoleTarget.Height / 2 - DEFAULT_CIRCLE_WIDTH / 2 - highlight.HolePadding;
			}
			else
			{
				calculatedTop = viewHoleTarget.Top - highlight.HolePadding;
			}

			return calculatedTop;			
		}

		nfloat CalculateWidth(CGRect viewHoleTarget, Highlight highlight)
		{
			nfloat calculatedWidth;
			if (highlight.HighlightStyle == Highlight.Style.Circle)
			{
				calculatedWidth = highlight.HoleWidth != Highlight.NOT_SET
										  ? highlight.HoleWidth
										  : DEFAULT_CIRCLE_WIDTH;
			}
			else
			{
				calculatedWidth = highlight.HoleWidth != Highlight.NOT_SET
										  ? highlight.HoleWidth + highlight.HolePadding * 2
										  : viewHoleTarget.Width + highlight.HolePadding * 2;
			}

			return calculatedWidth;
		}

		nfloat CalculateHeight(CGRect viewHoleTarget, Highlight highlight)
		{
			nfloat calculatedHeight;
			if (highlight.HighlightStyle == Highlight.Style.Circle)
			{
				calculatedHeight = highlight.HoleWidth != Highlight.NOT_SET ? highlight.HoleWidth : DEFAULT_CIRCLE_WIDTH;
			}
			else
			{
				calculatedHeight = viewHoleTarget.Height + highlight.HolePadding * 2;
			}

			return calculatedHeight;			
		}

        public override void Draw(CGRect rect)
        {
			BackgroundColor.SetFill();

            UIGraphics.RectFill(rect);

            var layer = new CAShapeLayer();

            var path = new CGPath();
            CGPath subPath = null;
            int index = 0;
            Highlight highlight = null;
            foreach(var calculatedHoleRect in CalculatedHoleRects)
            {
                if (!calculatedHoleRect.IsEmpty)
                {
					highlight = _helpItems[index].Highlight;
					subPath = new CGPath();
					// Make hole in view's overlay
					// NOTE: Here, instead of using the transparentHoleView UIView we could use a specific CFRect location instead...
					CGRect holeRect;
					if (_helpItems[index].Highlight.HighlightStyle == Highlight.Style.Circle)
					{
						subPath.AddArc(calculatedHoleRect.Left + highlight.HoleWidth / 2,
									calculatedHoleRect.Top + highlight.HoleWidth / 2,
									calculatedHoleRect.Width / 2,
									0.0f,
									2 * 3.14f,
									false);
					}
					else
					{
						holeRect = new CGRect(
							calculatedHoleRect.Left,
							calculatedHoleRect.Top,
							calculatedHoleRect.Width,
							calculatedHoleRect.Height);

						if (highlight.HighlightStyle == Highlight.Style.Rectangle)
						{
							subPath.AddRect(holeRect);
						}
						else
						{
							subPath.AddRoundedRect(holeRect, 10, 10);
						}
					}
					path.AddPath(subPath);

				}
                index++;
			}

			path.AddRect(Bounds);
			layer.Path = path;
			layer.FillRule = CAShapeLayer.FillRuleEvenOdd;
			Layer.Mask = layer;
		}

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            if (OverlayClick != null)
            {
                OverlayClick();
            }
        }
    }
}
