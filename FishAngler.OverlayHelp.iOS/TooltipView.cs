using System;
using CoreGraphics;
using UIKit;

namespace FishAngler.OverlayHelp.iOS
{
    public class TooltipView : UIView
    {
        UILabel _titleLabel;
        UILabel _descriptionLabel;
        Tooltip _tooltip;
        UIView _tooltipBubble;
        Slice _tooltipArrow;

        public readonly nfloat PADDING = 15;
        public readonly nfloat TITLE_DESCRIPTION_SEPARATION = 10;
        public readonly nfloat SLICE_HEIGHT = 15;

        public TooltipView(Tooltip tooltip)
        {
            _tooltip = tooltip;

            _tooltipBubble = new UIView();
            _tooltipBubble.BackgroundColor = _tooltip.BackgroundColor;
            _tooltipBubble.Layer.CornerRadius = 5;
            _tooltipBubble.Layer.MasksToBounds = true;
            Add(_tooltipBubble);

            _titleLabel = new UILabel()
            {
                Text = tooltip.Title,
                TextColor = UIColor.White,
                Lines = 0,
                Font = UIFont.SystemFontOfSize(13)
            };
            _tooltipBubble.Add(_titleLabel);

            _descriptionLabel = new UILabel()
            {
                Text = tooltip.Description,
                TextColor = UIColor.White,
                Lines = 0,
                Font = UIFont.SystemFontOfSize(12)
            };
            _tooltipBubble.Add(_descriptionLabel);

            _tooltipArrow = new Slice()
            {
                Color = _tooltip.BackgroundColor.CGColor,
                SliceDirection = GetSliceDirection(_tooltip.Gravity)
            };
            Add(_tooltipArrow);
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

        nfloat GetXForSlice(int gravity, nfloat tooltipWidth, nfloat sliceWidth)
        {
            nfloat x = 0;
            if ((gravity & (int)GravityFlags.Left) == (int)GravityFlags.Left)
            {
                x =  tooltipWidth + 2;
            }
            else if ((gravity & (int)GravityFlags.Right) == (int)GravityFlags.Right)
            {
                x = -sliceWidth - 2;
            }
            else if ((gravity & (int)GravityFlags.Top) == (int)GravityFlags.Top)
            {
                x =  tooltipWidth / 2 - sliceWidth / 2;
            }
            else if ((gravity & (int)GravityFlags.Bottom) == (int)GravityFlags.Bottom)
            {
                x =  tooltipWidth / 2 - sliceWidth / 2;
            }
            return x;
        }

        nfloat GetYForSlice(int gravity, nfloat tooltipHeight, nfloat sliceHeight)
        {
            nfloat y = 0;
            if ((gravity & (int)GravityFlags.Left) == (int)GravityFlags.Left)
            {
                y =  tooltipHeight / 2 - sliceHeight / 2;
            }
            else if ((gravity & (int)GravityFlags.Right) == (int)GravityFlags.Right)
            {
                y = tooltipHeight / 2 - sliceHeight / 2;
            }
            else if ((gravity & (int)GravityFlags.Top) == (int)GravityFlags.Top)
            {
                y = tooltipHeight + 2;
            }
            else if ((gravity & (int)GravityFlags.Bottom) == (int)GravityFlags.Bottom)
            {
                y = -sliceHeight - 2;
            }
            return y;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _tooltipBubble.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);

            var titleSize = _tooltip.Title.StringSize(
                _titleLabel.Font, 
                new CGSize(Frame.Width - PADDING * 2, 10000), UILineBreakMode.WordWrap);
            var descriptionSize = _tooltip.Description.StringSize(_descriptionLabel.Font, new CGSize(Frame.Width - PADDING * 2, 10000), UILineBreakMode.WordWrap);

            var titleHeight = string.IsNullOrEmpty(_tooltip.Title) ? 0 : titleSize.Height;
            _titleLabel.Frame = new CGRect(PADDING, PADDING, Frame.Width - PADDING * 2, titleHeight);
            _titleLabel.Hidden = string.IsNullOrEmpty(_tooltip.Title);

            var descriptionTop = string.IsNullOrEmpty(_tooltip.Title) ? PADDING : _titleLabel.Frame.Bottom + TITLE_DESCRIPTION_SEPARATION;
            _descriptionLabel.Frame = new CGRect(PADDING, descriptionTop, Frame.Width - PADDING * 2, descriptionSize.Height);

            var sliceWidth = 15;
            var xSlice = GetXForSlice(_tooltip.Gravity, Frame.Width, sliceWidth);
            var ySlice = GetYForSlice(_tooltip.Gravity, Frame.Height, sliceWidth);
            _tooltipArrow.Frame = new CGRect(xSlice, ySlice, sliceWidth, sliceWidth);
        }

        internal nfloat HeightThatFits(nfloat width)
        {
            var titleSize = _tooltip.Title.StringSize(_titleLabel.Font, new CGSize(width - PADDING * 2, 10000), UILineBreakMode.WordWrap);
            var descriptionSize = _tooltip.Description.StringSize(_descriptionLabel.Font, new CGSize(width - PADDING * 2, 10000), UILineBreakMode.WordWrap);

            var titleHeight = string.IsNullOrEmpty(_tooltip.Title) ? 0 : titleSize.Height + TITLE_DESCRIPTION_SEPARATION;

            var height = titleHeight + descriptionSize.Height + PADDING * 2;

            return height;
        }
    }
}
