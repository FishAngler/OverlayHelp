using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace FishAngler.OverlayHelp.Android
{
    public class TooltipView : LinearLayout
    {
        Tooltip _tooltip;

        public TooltipView(Context context, Tooltip tooltip) : base(context)
        {
            _tooltip = tooltip;
            SetupTooltip();
        }

		void SetupTooltip()
		{
            if (_tooltip != null)
			{
				/* inflate and get views */
                var parent = (ViewGroup)((Activity)Context).Window.DecorView;
				var layoutInflater = ((Activity)Context).LayoutInflater;

				layoutInflater.Inflate(Resource.Layout.TooltipLayout, this);

				View toolTipContainer = FindViewById(Resource.Id.toolTip_container);
				TextView toolTipTitleTV = (TextView)FindViewById(Resource.Id.title);
				TextView toolTipDescriptionTV = (TextView)FindViewById(Resource.Id.description);

				/* set tooltip attributes */
                SetBackgroundColor(_tooltip.BackgroundColor);
				if (_tooltip.Background != null)
				{
					Background = _tooltip.Background;
				}

				toolTipTitleTV.SetTextColor(_tooltip.TextColor);

				toolTipDescriptionTV.SetTextColor(_tooltip.TextColor);

				if (string.IsNullOrEmpty(_tooltip.Title))
				{
					toolTipTitleTV.Visibility = ViewStates.Gone;
				}
				else
				{
					toolTipTitleTV.Visibility = ViewStates.Visible;
					toolTipTitleTV.Text = _tooltip.Title;
				}

				if (string.IsNullOrEmpty(_tooltip.Description))
				{
					toolTipDescriptionTV.Visibility = ViewStates.Gone;
				}
				else
				{
					toolTipDescriptionTV.Visibility = ViewStates.Visible;
					toolTipDescriptionTV.Text = _tooltip.Description;
				}
			}
		}

	}
}
