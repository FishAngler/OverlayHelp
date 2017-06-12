using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace FishAngler.OverlayHelp.iOS
{
    public class Slice : UIView
    {
		public enum Direction
		{
			North, South, East, West
		}

        public Direction SliceDirection { get; set; } = Direction.South;

        public CGColor Color { get; set; }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var colorDotLayer = new CAShapeLayer();
            Layer.AddSublayer(colorDotLayer);
            colorDotLayer.FillColor = Color;
            CGRect bounds = Bounds;
            nfloat radius = (Bounds.Size.Width - 6) / 2;
            nfloat a = radius * (nfloat)Math.Sqrt(3.0) / 2;
            nfloat b = radius / 2;

            var path = new UIBezierPath();
			path.MoveTo(new CGPoint(0, -radius));
			path.AddLineTo(new CGPoint(a, b));
			path.AddLineTo(new CGPoint(-a, b));

            var pi = 3.14159f;
			path.ClosePath();
			if (SliceDirection == Direction.North)
            {
                path.ApplyTransform(CGAffineTransform.MakeTranslation(Bounds.GetMidX(), Bounds.Height));   
            }            							
			else if (SliceDirection == Direction.South)
			{
                path.ApplyTransform(CGAffineTransform.MakeTranslation(-Bounds.GetMidX(), 0));
				path.ApplyTransform(CGAffineTransform.MakeRotation(180 * (pi / 180)));
			}
			else if (SliceDirection == Direction.East)
			{
                path.ApplyTransform(CGAffineTransform.MakeTranslation(Bounds.GetMidX(), 0));
                path.ApplyTransform(CGAffineTransform.MakeRotation(90* (pi / 180)));
			}
			else if (SliceDirection == Direction.West)
			{
                path.ApplyTransform(CGAffineTransform.MakeTranslation(-Bounds.GetMidX(), Bounds.Height));
                path.ApplyTransform(CGAffineTransform.MakeRotation(270 * (pi / 180)));
			}

			colorDotLayer.Path = path.CGPath;
        }
    }
}
