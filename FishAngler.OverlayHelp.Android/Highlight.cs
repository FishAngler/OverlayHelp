using System;
namespace FishAngler.OverlayHelp.Android
{
    public class Highlight
    {
		public const int NOT_SET = -1;

		public enum Style
		{
			Circle, Rectangle, RoundedRectangle, NoHole
		}

        public Highlight() : this(Style.Circle) {}

        public Highlight(Style style)
        {
            HighlightStyle = style;
        }

		public Style HighlightStyle { get; set; }
		public int HoleWidth { get; set; } = NOT_SET;
		public int HolePadding { get; set; } = 0;
		public int RoundedCornerRadius { get; set; } = 0;

		public Highlight SetHoleWidth(int holeWidth)
		{
			HoleWidth = holeWidth;
			return this;
		}

		public Highlight SetHolePadding(int padding)
		{
			HolePadding = padding;
			return this;
		}

		public Highlight SetRoundedCornerRadius(int roundedCornerRadius)
		{
			RoundedCornerRadius = roundedCornerRadius;
			return this;
		}
	}
}
