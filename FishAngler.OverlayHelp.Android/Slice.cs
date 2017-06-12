using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using static Android.Graphics.Paint;

namespace FishAngler.OverlayHelp.Android
{
    public class Slice : View
	{
        Paint _paint;
        Path _path;

		public enum Direction
		{
			North, South, East, West
		}

        public Slice(Context context) : base(context)
		{
			Create();
		}

        public Direction SliceDirection { get; set; } = Direction.South;


        public Color Color
        {
            get { return _paint.Color; }
            set 
            { 
                _paint.Color = value;
				Invalidate();
			}
        }

		void Create()
		{
			_paint = new Paint();
			_paint.SetStyle(Style.Fill);
			_paint.Color = Color.Red;
		}

		protected override void OnDraw(Canvas canvas)
		{
			_path = calculate(SliceDirection);
			canvas.DrawPath(_path, _paint);
		}

		Path calculate(Direction direction)
		{
			Point p1 = null, p2 = null, p3 = null;

			if (direction == Direction.North)
			{
                p1 = new Point((Width / 2), 0);
                p2 = new Point(Width, Height);
				p3 = new Point(0, Height);
			}
			else if (direction == Direction.South)
			{
                p1 = new Point(0, 0);
				p2 = new Point(p1.X + Width, p1.Y);
				p3 = new Point(p1.X + (Width / 2), p1.Y + Height);
			}
			else if (direction == Direction.East)
			{
				p1 = new Point(0, 0);
				p2 = new Point(p1.X, p1.Y + Height);
				p3 = new Point(p1.X + Width, p1.Y + (Height / 2));
			}
			else if (direction == Direction.West)
			{
				p1 = new Point(Width, 0);
				p2 = new Point(0, p1.Y + Height / 2);
				p3 = new Point(Width, p2.Y + (Height / 2));
			}

			Path path = new Path();
			path.MoveTo(p1.X, p1.Y);
			path.LineTo(p2.X, p2.Y);
			path.LineTo(p3.X, p3.Y);

			return path;
		}
	}
}
