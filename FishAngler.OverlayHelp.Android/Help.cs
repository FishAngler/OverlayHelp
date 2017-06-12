using Android.App;
using Android.Views;
using Android.Support.V4.View;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using static Android.Views.View;
using System.Collections.Generic;
using System.Linq;

namespace FishAngler.OverlayHelp.Android
{
    public class Help : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener, View.IOnClickListener
    {
		Activity _context;
		List<HelpItem> _helpItems = new List<HelpItem>();
        List<View> _targetViews = new List<View>();
		List<View> _addedViews = new List<View>();
        bool _visible;
        const int SLICE_WIDTH_DP = 12;
        Overlay _overlay = new Overlay();

        public Help(Activity context)
        {
            _context = context;
        }

        public Help AddControl(View targetView, Tooltip tooltip, Highlight highlight = null)
        {
            if (!_targetViews.Contains(targetView))
            {
                _targetViews.Add(targetView);  
            }

			_helpItems.Add(
                new HelpItem(targetView) { Highlight = highlight, Tooltip = tooltip }
			);
			
            return this;
        }

		public Help AddRect(Rect targetRect, Tooltip tooltip, Highlight highlight = null)
		{
			if (_helpItems.Any(hi => hi.TargetRect != null && hi.TargetRect.Equals(targetRect)))
			{
				return this;
			}

			_helpItems.Add(
				new HelpItem(targetRect) { Highlight = highlight, Tooltip = tooltip }
			);

			return this;
		}

        public void Play()
        {
			if (_visible)
			{
				return;
			}
			_visible = true;

            SetupView();
        }

		public void Hide()
		{
			foreach (View view in _addedViews)
			{
                if (view.Parent != null)
                {
                    ((ViewGroup)view.Parent).RemoveView(view);
                }
			}

			_addedViews.Clear();
			_visible = false;
		}

		public Help SetOverlay(Overlay overlay)
        {
            _overlay = overlay;
            return this;
        }

        protected void SetupView()
        {
			// This can only be setup after all the views is ready and obtain it's position/measurement
			// so when this is the 1st time TourGuide is being added,
			// else block will be executed, and ViewTreeObserver will make TourGuide setup process to be delayed until everything is ready
			// when this is run the 2nd or more times, if block will be executed

            var pendingViews = _targetViews.Count(view => !ViewCompat.IsAttachedToWindow(view));

			if (_targetViews.Count == 0 || pendingViews == 0)
            {
                StartView();
            }
            else
            {
                _targetViews.Where(view => !ViewCompat.IsAttachedToWindow(view))
                            .ToList()
                            .ForEach(view =>
                            {
								ViewTreeObserver viewTreeObserver = view.ViewTreeObserver;
								viewTreeObserver.AddOnGlobalLayoutListener(this);
							});
            }
        }

        public void OnGlobalLayout()
        {
			var pendingViews = _targetViews.Count(view => !ViewCompat.IsAttachedToWindow(view));

			//TODO: check what to do with 
			if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
			{
				_targetViews.Where(view => ViewCompat.IsAttachedToWindow(view))
							.ToList()
							.ForEach(view =>
							{
								//noinspection deprecation
								view.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
							});
			}
			else
			{
				_targetViews.Where(view => ViewCompat.IsAttachedToWindow(view))
							.ToList()
							.ForEach(view =>
							{
								view.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
							});
			}

			if (pendingViews == 0)
            {
				StartView();
			}
        }

        void StartView()
        {
			var overlayWithHoleView = new OverlayWithHoleView(_context, _helpItems, _overlay);
            _addedViews.Add(overlayWithHoleView);

            /* handle click disable */
            HandleDisableClicking(overlayWithHoleView);

            SetupOverlayLayout(overlayWithHoleView);
            /* setup tooltip view */
            SetupTooltips(overlayWithHoleView);
        }

        void HandleDisableClicking(OverlayWithHoleView overlayWithHoleView)
        {
			overlayWithHoleView.SetOnClickListener(this);
			
            if (_overlay != null && _overlay.OnClickListener != null)
            {
                overlayWithHoleView.Clickable = true;
            }
        }

        void SetupOverlayLayout(OverlayWithHoleView overlayWithHoleView)
        {
            FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            ViewGroup contentArea = (ViewGroup)_context.Window.DecorView.FindViewById(global::Android.Resource.Id.Content);
            int[] pos = new int[2];
            contentArea.GetLocationOnScreen(pos);
            // frameLayoutWithHole's coordinates are calculated taking full screen height into account
            // but we're adding it to the content area only, so we need to offset it to the same Y value of contentArea

            layoutParams.SetMargins(0, -pos[1], 0, 0);
            contentArea.AddView(overlayWithHoleView, layoutParams);
        }

        void SetupTooltips(OverlayWithHoleView overlayWithHoleView)
        {
			float density = _context.Resources.DisplayMetrics.Density;

			var parent = (ViewGroup)_context.Window.DecorView;

			var index = 0;
			foreach (var helpItem in _helpItems)
			{
				var layoutParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
				var layoutInflater = _context.LayoutInflater;

                var tooltipView = new TooltipView(_context, helpItem.Tooltip);

                if (helpItem.Tooltip.Width != -1)
                {
                    layoutParams.Width = helpItem.Tooltip.Width;
                }

				parent.AddView(tooltipView, layoutParams);

				tooltipView.StartAnimation(helpItem.Tooltip.EnterAnimation);
                var reference = helpItem.Highlight != null ? overlayWithHoleView.CalculatedHoleRects[index] : helpItem.TargetRect;
                var tooltipRect = GetTooltipRect(reference, helpItem.Tooltip, tooltipView as TooltipView, helpItem.Highlight);

                // pass tooltip onClickListener into tooltipViewGroup
                if (helpItem.Tooltip.OnClickListener != null)
                {
                    tooltipView.SetOnClickListener(helpItem.Tooltip.OnClickListener);
                }

                // set the position using setMargins on the left and top
                layoutParams.SetMargins(tooltipRect.Left, tooltipRect.Top, 0, 0);


                // TODO: move arrow draw to TooltipView as we have for iOS
				int sliceWidth = (int)(SLICE_WIDTH_DP * density);
				var layoutParamsSlice = new FrameLayout.LayoutParams(sliceWidth, sliceWidth);
				var slice = new Slice(_context);
                slice.Color = helpItem.Tooltip.BackgroundColor;
                slice.SliceDirection = GetSliceDirection(helpItem.Tooltip.Gravity);

				parent.AddView(slice, layoutParamsSlice);
                var xSlice = GetXForSlice(helpItem.Tooltip.Gravity, tooltipRect.Left, tooltipRect.Width(), sliceWidth);
                var ySlice = GetYForSlice(helpItem.Tooltip.Gravity, tooltipRect.Top, tooltipRect.Height(), sliceWidth);
                layoutParamsSlice.SetMargins(xSlice, ySlice, 0, 0);

				/* add shadow if it's turned on */
                slice.StartAnimation(helpItem.Tooltip.EnterAnimation);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    tooltipView.Elevation = helpItem.Tooltip.Shadow ? 6 : 0;
					slice.Elevation = helpItem.Tooltip.Shadow ? 6 : 0;
				}
                _addedViews.Add(tooltipView);
                _addedViews.Add(slice);
				index++;
			}
		}
       
        Rect GetTooltipRect(Rect reference, Tooltip toolpit, TooltipView tooltipView, Highlight highlight)
        {
            var tooltipRect = new Rect();

			// get measured size of tooltip
			var measureSpecWidth = MeasureSpec.MakeMeasureSpec(
				toolpit.Width != -1 ? toolpit.Width : FrameLayout.LayoutParams.WrapContent,
				MeasureSpecMode.Exactly);
			var measureSpecHeight = MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
			tooltipView.Measure(measureSpecWidth, measureSpecHeight);

			int tooltipWidth = toolpit.Width != -1 ? toolpit.Width : tooltipView.MeasuredWidth;
			int tooltipHeight = tooltipView.MeasuredHeight;

			float density = _context.Resources.DisplayMetrics.Density;

			// calculate x position, based on gravity, tooltipWidth, parent max width, x position of target view, adjustment
			var parent = (ViewGroup)_context.Window.DecorView;

			if (tooltipWidth > parent.Width)
			{
                tooltipRect.Left = GetXForTooTip(reference, toolpit.Gravity, parent.Width);
			}
			else
			{
				tooltipRect.Left = GetXForTooTip(reference, toolpit.Gravity, tooltipWidth);
			}

            tooltipRect.Top = GetYForTooTip(reference, toolpit.Gravity, tooltipHeight, highlight);

			// 1. width < screen check
			if (tooltipWidth > parent.Width)
			{
				tooltipView.LayoutParameters.Width = parent.Width;
				tooltipWidth = parent.Width;
			}
			// 2. x left boundary check
			if (tooltipRect.Left < 0)
			{
				tooltipRect.Left = 0;
			}
			// 3. x right boundary check
			int tempRightX = tooltipRect.Left + tooltipWidth;
			if (tempRightX > parent.Width)
			{
				tooltipRect.Left = parent.Width - tooltipWidth;
			}

            tooltipRect.Right = tooltipRect.Left + tooltipWidth;
            tooltipRect.Bottom = tooltipRect.Top + tooltipHeight;

            return tooltipRect;
		}

        Slice.Direction GetSliceDirection(int gravity)
        {
            var sliceDirection = Slice.Direction.South;
            if ((gravity & (int)GravityFlags.Left) == (int)GravityFlags.Left)
            {
                sliceDirection = Slice.Direction.East;
            }
			else if ((gravity & (int)GravityFlags.Right) == (int)GravityFlags.Right)
			{
				sliceDirection = Slice.Direction.West;
			}
			else if ((gravity & (int)GravityFlags.Top) == (int)GravityFlags.Top)
			{
				sliceDirection = Slice.Direction.South;
			}
			else if ((gravity & (int)GravityFlags.Bottom) == (int)GravityFlags.Bottom)
			{
				sliceDirection = Slice.Direction.North;
			}

            return sliceDirection;
		}

        int GetXForSlice(int gravity, int tooltipLeft, int tooltipWidth, int sliceWidth)
        {
            int x = 0;
			if ((gravity & (int)GravityFlags.Left) == (int)GravityFlags.Left)
			{
				x = tooltipLeft + tooltipWidth;
			}
			else if ((gravity & (int)GravityFlags.Right) == (int)GravityFlags.Right)
			{
				x = tooltipLeft - sliceWidth;
			}
			else if ((gravity & (int)GravityFlags.Top) == (int)GravityFlags.Top)
			{
				x = tooltipLeft + tooltipWidth / 2 - sliceWidth / 2;
			}
			else if ((gravity & (int)GravityFlags.Bottom) == (int)GravityFlags.Bottom)
			{
				x = tooltipLeft + tooltipWidth / 2 - sliceWidth / 2;
			}
            return x;
        }

        int GetYForSlice(int gravity, int tooltipTop, int tooltipHeight, int sliceHeight)
		{
            int y = 0;
			if ((gravity & (int)GravityFlags.Left) == (int)GravityFlags.Left)
			{
				y = tooltipTop + tooltipHeight / 2 - sliceHeight / 2;
			}
			else if ((gravity & (int)GravityFlags.Right) == (int)GravityFlags.Right)
			{
				y = tooltipTop + tooltipHeight / 2 - sliceHeight / 2;
			}
			else if ((gravity & (int)GravityFlags.Top) == (int)GravityFlags.Top)
			{
				y = tooltipTop + tooltipHeight;
			}
			else if ((gravity & (int)GravityFlags.Bottom) == (int)GravityFlags.Bottom)
			{
				y = tooltipTop - sliceHeight;
			}
			return y;
		}

		int GetXForTooTip(Rect reference, int gravity, int tooltipWidth)
		{
            float density = _context.Resources.DisplayMetrics.Density;
			float adjustment = (15 + SLICE_WIDTH_DP) * density;

			int x;
			if ((gravity & (int)GravityFlags.Left) == (int)GravityFlags.Left)
			{
				x = reference.Left - tooltipWidth - (int)adjustment;
			}
			else if ((gravity & (int)GravityFlags.Right) == (int)GravityFlags.Right)
			{
				x = reference.Left + reference.Width() + (int)adjustment;
			}
			else
			{
				x = reference.Left + reference.Width() / 2 - tooltipWidth / 2;
			}
			return x;
		}

		int GetYForTooTip(Rect reference, int gravity, int tooltipHeight, Highlight highlight)
		{
			float density = _context.Resources.DisplayMetrics.Density;
            float adjustment = (15 + SLICE_WIDTH_DP) * density;

			if (highlight != null)
			{
				adjustment += highlight.HighlightStyle == Highlight.Style.Circle ? highlight.HoleWidth / 2 : reference.Height() / 2;
			}

			int y;
			int highlightedViewCenterY = reference.Top + (reference.Height() / 2);
			if ((gravity & (int)GravityFlags.Top) == (int)GravityFlags.Top)
			{
				y = highlightedViewCenterY - tooltipHeight - (int)adjustment;
			}
			else if ((gravity & (int)GravityFlags.Bottom) == (int)GravityFlags.Bottom)
			{
				y = highlightedViewCenterY + (int)adjustment;
			}
			else // centered 
			{
				y = highlightedViewCenterY - tooltipHeight / 2;
			}
			return y;
		}

        public void OnClick(View v) 
        {
            _overlay.OnClickListener?.Invoke();
        }
    }
}
