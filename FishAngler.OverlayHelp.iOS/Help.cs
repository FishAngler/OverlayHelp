﻿﻿﻿﻿﻿﻿using System; using System.Collections.Generic; using System.Linq;
using CoreGraphics; using UIKit;  namespace FishAngler.OverlayHelp.iOS {     public class Help     {
		UIView _context;
		List<HelpItem> _helpItems = new List<HelpItem>();         List<UIView> _addedViews = new List<UIView>();         Overlay _overlay = new Overlay();         bool _visible;          const int SLICE_WIDTH_DP = 12;         const float FADE_DURATION = 0.5f;          public Help(UIView context)         {           _context = context;         }          public Help AddControl(UIView targetView, Tooltip tooltip, Highlight highlight = null)         {             CGRect targetRect = CGRect.Empty;
			if (targetView != null)
			{
                var absoluteFrame = targetView.ConvertRectToView(targetView.Bounds, _context);
                targetRect = absoluteFrame;
			}

            if (_helpItems.Any(hi => hi.TargetRect.Equals(targetRect)))             {                 return this;             }              _helpItems.Add(                 new HelpItem() { TargetRect = targetRect, Highlight = highlight, Tooltip = tooltip }             );              return this;         }          public Help AddRect(CGRect targetRect, Tooltip tooltip, Highlight highlight = null)         {
			if (_helpItems.Any(hi => hi.TargetRect.Equals(targetRect)))
			{
				return this;
			}

			_helpItems.Add(
				new HelpItem() { TargetRect = targetRect, Highlight = highlight, Tooltip = tooltip }
			);
             return this;         }                      public Help SetOverlay(Overlay overlay)         {             if (overlay != null)             {                 _overlay = overlay;                }              return this;         }          public void Play()         {             if (_visible)             {
                return;             }             _visible = true;             SetupView();         }          public void Hide()         {             UIView.Animate(FADE_DURATION, 0, UIViewAnimationOptions.CurveEaseInOut, () =>             {
                foreach (UIView view in _addedViews)                 {                     view.Alpha = 0;                 }             },             () =>              {
                foreach (UIView view in _addedViews)
                {
                    view.RemoveFromSuperview();
                }

                _addedViews.Clear();
                _helpItems.Clear();
                _visible = false;             });         }          void SetupView()         {             StartView();         }          void StartView()         { 			var overlayWithHoleView = new OverlayWithHoleView(_helpItems, _overlay)
			{
				Frame = new CGRect(_context.Frame.Left, _context.Frame.Top, _context.Frame.Width, _context.Frame.Height),                 Alpha = 0
			};             overlayWithHoleView.OverlayClick += _overlay.OnClickListener; 
			_context.Add(overlayWithHoleView);             _addedViews.Add(overlayWithHoleView);
             overlayWithHoleView.Fade(true, FADE_DURATION); 
            var index = 0;
			foreach(var helpItem in _helpItems)             { 	            var tooltipView = new TooltipView(helpItem.Tooltip);                 var reference = helpItem.Highlight != null ? overlayWithHoleView.CalculatedHoleRects[index] : helpItem.TargetRect; 	            var tooltipRect = GetTooltipRect(reference, helpItem.Tooltip, tooltipView as TooltipView, helpItem.Highlight);
				tooltipView.Frame = tooltipRect;

				_context.Add(tooltipView); 	            if(helpItem.Tooltip.UseEnterAnimation) 	            { 	               tooltipView.Fade(true, FADE_DURATION); 	            }                  _addedViews.Add(tooltipView);                 index++;
			}         }          CGRect GetTooltipRect(CGRect reference, Tooltip toolpit, TooltipView tooltipView, Highlight highlight)         {             var height = tooltipView.HeightThatFits(toolpit.Width);             var rect = new CGRect();              if (toolpit.Width > _context.Frame.Width)             {               rect.X = GetXForTooTip(reference, toolpit.Gravity, _context.Frame.Width);             }             else             {               rect.X = GetXForTooTip(reference, toolpit.Gravity, toolpit.Width);             }              rect.Y = GetYForTooTip(reference, toolpit.Gravity, height, highlight);             rect.Width = _context.Frame.Width < toolpit.Width ? _context.Frame.Width : toolpit.Width;             rect.Height = height;              // 1. width < screen check             if (rect.Width > _context.Frame.Width)             {                 rect.Width = _context.Frame.Width;             }              // 2. x left boundary check             if (rect.X < 0)             {               rect.X = 0;             }              // 3. x right boundary check             var tempRightX = rect.Left + rect.Width;             if (tempRightX > _context.Frame.Width)             {                 rect.X = _context.Frame.Width - rect.Width;             }              return rect;         }          nfloat GetXForTooTip(CGRect reference, int gravity, nfloat tooltipWidth)         {             nfloat adjustment = 20;                          nfloat x;             if ((gravity & (int)GravityFlags.Left) == (int)GravityFlags.Left)             {                 x = reference.Left - tooltipWidth - (int)adjustment;             }             else if ((gravity & (int)GravityFlags.Right) == (int)GravityFlags.Right)             {                 x = reference.Left + reference.Width + (int)adjustment;             }             else             {                 x = reference.Left + reference.Width / 2 - tooltipWidth / 2;             }             return x;         }          nfloat GetYForTooTip(CGRect reference, int gravity, nfloat tooltipHeight, Highlight highlight)         {             nfloat adjustment = 0;              if (highlight != null)             {
				adjustment = highlight.HighlightStyle == Highlight.Style.Circle ? highlight.HoleWidth / 2 : reference.Height / 2;
				adjustment += 20; 			}             else              {                 adjustment = 40;             }              nfloat y;             nfloat highlightedViewCenterY = reference.Top + (reference.Height / 2);             if ((gravity & (int)GravityFlags.Top) == (int)GravityFlags.Top)             {                 y = highlightedViewCenterY - tooltipHeight - (int)adjustment;             }             else if ((gravity & (int)GravityFlags.Bottom) == (int)GravityFlags.Bottom)             {                 y = highlightedViewCenterY + (int)adjustment;             }             else // centered              {                 y = highlightedViewCenterY - tooltipHeight / 2;             }             return y;         }     } }  