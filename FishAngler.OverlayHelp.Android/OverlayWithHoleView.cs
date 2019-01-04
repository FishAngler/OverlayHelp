using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace FishAngler.OverlayHelp.Android
{
	public class OverlayWithHoleView : ImageView
	{
        const int DEFAULT_CIRCLE_WIDTH = 50;
		float _density;
	    Overlay _overlay;
		IList<HelpItem> _helpItems; // This is the list of views to be highlighted, where the holes should be placed

		public OverlayWithHoleView(Context context, IList<HelpItem> helpItems, Overlay overlay) : base(context)
        {
			SetLayerType(LayerType.Software, null);

			_helpItems = helpItems;
			_overlay = overlay;

			_density = context.Resources.DisplayMetrics.Density;
		}


		public IList<Rect> CalculatedHoleRects
		{
			get
			{
				var calculatedHoleRects = new List<Rect>();
				foreach (var helpItem in _helpItems)
				{
					if (helpItem.Highlight != null)
					{
						var rect = new Rect(
							CalculateLeft(helpItem.TargetRect, helpItem.Highlight),
							CalculateTop(helpItem.TargetRect, helpItem.Highlight),
							CalculateLeft(helpItem.TargetRect, helpItem.Highlight) + CalculateWidth(helpItem.TargetRect, helpItem.Highlight),
							CalculateTop(helpItem.TargetRect, helpItem.Highlight) + CalculateHeight(helpItem.TargetRect, helpItem.Highlight));
						calculatedHoleRects.Add(rect);

					}
					else
					{
                        calculatedHoleRects.Add(new Rect());
					}
				}
				return calculatedHoleRects;
			}
		}

        public int CalculateLeft(Rect viewHoleTarget, Highlight highlight)
        {
			int calculatedLeft;
			if (highlight.HighlightStyle == Highlight.Style.Circle)
			{
				calculatedLeft = highlight.HoleWidth != Highlight.NOT_SET
										 ? viewHoleTarget.Left + viewHoleTarget.Width() / 2 - highlight.HoleWidth / 2
										 : viewHoleTarget.Left + viewHoleTarget.Width() / 2 - DEFAULT_CIRCLE_WIDTH / 2;
			}
			else
			{
				calculatedLeft = highlight.HoleWidth != Highlight.NOT_SET
										 ? viewHoleTarget.Left + viewHoleTarget.Width() / 2 - highlight.HoleWidth / 2 - highlight.HolePadding
										 : viewHoleTarget.Left - highlight.HolePadding;
			}

			return calculatedLeft;
		}

		int CalculateTop(Rect viewHoleTarget, Highlight highlight)
		{
			int calculatedTop;
			if (highlight.HighlightStyle == Highlight.Style.Circle)
			{
				calculatedTop = highlight.HoleWidth != Highlight.NOT_SET
										? viewHoleTarget.Top + viewHoleTarget.Height() / 2 - highlight.HoleWidth / 2 - highlight.HolePadding
										: viewHoleTarget.Top + viewHoleTarget.Height() / 2 - DEFAULT_CIRCLE_WIDTH / 2 - highlight.HolePadding;
			}
			else
			{
				calculatedTop = viewHoleTarget.Top - highlight.HolePadding;
			}

			return calculatedTop;
		}

		int CalculateWidth(Rect viewHoleTarget, Highlight highlight)
		{
			int calculatedWidth;
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
										  : viewHoleTarget.Width() + highlight.HolePadding * 2;
			}

			return calculatedWidth;
		}


		int CalculateHeight(Rect viewHoleTarget, Highlight highlight)
		{
			int calculatedHeight;
			if (highlight.HighlightStyle == Highlight.Style.Circle)
			{
				calculatedHeight = highlight.HoleWidth != Highlight.NOT_SET ? highlight.HoleWidth : DEFAULT_CIRCLE_WIDTH;
			}
			else
			{
				calculatedHeight = viewHoleTarget.Height() + highlight.HolePadding * 2;
			}

			return calculatedHeight;
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
			var paint = new Paint(PaintFlags.AntiAlias);
			paint.Color = _overlay.BackgroundColor;
			paint.SetStyle(Paint.Style.Fill);
			canvas.DrawPaint(paint);

            var parent = (ViewGroup)((Activity)Context).Window.DecorView;
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));

			int index = 0;
			Highlight highlight = null;
            foreach (var calculatedHoleRect in CalculatedHoleRects)
            {
                if (!calculatedHoleRect.IsEmpty)
                {
                    highlight = _helpItems[index].Highlight;
                    if (highlight.HighlightStyle == Highlight.Style.Circle)
                    {
                        canvas.DrawCircle(
                                calculatedHoleRect.Left + highlight.HoleWidth / 2,
                                calculatedHoleRect.Top + highlight.HoleWidth / 2,
                                calculatedHoleRect.Width() / 2,
                                paint);

                    }
                    else if (highlight.HighlightStyle == Highlight.Style.Rectangle || highlight.HighlightStyle == Highlight.Style.RoundedRectangle)
                    {
                        int holeWidth = highlight.HoleWidth != Highlight.NOT_SET ? highlight.HoleWidth : DEFAULT_CIRCLE_WIDTH;
                        holeWidth = holeWidth + highlight.HolePadding * 2 < parent.Width ? holeWidth : parent.Width - highlight.HolePadding * 2;

                        if (highlight.HighlightStyle == Highlight.Style.Rectangle)
                        {
                            canvas.DrawRect(
                                    calculatedHoleRect.Left,
                                    calculatedHoleRect.Top,
                                    calculatedHoleRect.Left + calculatedHoleRect.Width(),
                                    calculatedHoleRect.Top + calculatedHoleRect.Height(),
                                    paint);
                        }
                        else if (highlight.HighlightStyle == Highlight.Style.RoundedRectangle)
                        {
                            int roundedCornerRadiusPx;
                            if (highlight.RoundedCornerRadius != 0)
                            {
                                roundedCornerRadiusPx = highlight.RoundedCornerRadius;
                            }
                            else
                            {
                                roundedCornerRadiusPx = (int)(10 * _density);
                            }

                            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                            {
                                canvas.DrawRoundRect(
                                    calculatedHoleRect.Left,
                                    calculatedHoleRect.Top,
                                    calculatedHoleRect.Left + calculatedHoleRect.Width(),
                                    calculatedHoleRect.Top + calculatedHoleRect.Height(),
                                    roundedCornerRadiusPx,
                                    roundedCornerRadiusPx,
                                    paint);
                            }
                            else
                            {
                                canvas.DrawRect(
                                        calculatedHoleRect.Left,
                                        calculatedHoleRect.Top,
                                        calculatedHoleRect.Left + calculatedHoleRect.Width(),
                                        calculatedHoleRect.Top + calculatedHoleRect.Height(),
                                        paint);
                            }
                        }
                    }
				}
				index++;
			}
        }

        public void CleanUp()
        {
            if (Parent != null)
            {
                ((ViewGroup)Parent).RemoveView(this);   
            }
        }

		public override bool DispatchTouchEvent(MotionEvent ev)
		{
			//first check if the location button should handle the touch event
            if (_helpItems != null && _helpItems.Count > 0)
			{
				if (IsWithinViewHole(ev) && _overlay != null && _overlay.DisableClickThroughHole)
				{
                    // block it but execute the overlay click handler
                    _overlay.OnClickAction?.Invoke();
                    return true;
				}
				else if (IsWithinViewHole(ev))
				{
					// let it pass through
					return false;
				}
			}
			// do nothing, just propagating up to super
			return base.DispatchTouchEvent(ev);
		}

		bool IsWithinViewHole(MotionEvent ev)
		{
            var res = CalculatedHoleRects.Any(re => ev.GetY() >= re.Top &&
                    ev.GetY() <= (re.Top + re.Height()) &&
                    ev.GetX() >= re.Left &&
                    ev.GetX() <= (re.Left + re.Width()));

            return res;
		}
    }
}
