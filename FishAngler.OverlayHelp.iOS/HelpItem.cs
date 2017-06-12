using System;
using CoreGraphics;
using UIKit;

namespace FishAngler.OverlayHelp.iOS
{
    public class HelpItem
    {
        public CGRect TargetRect { get; set; }
		public Tooltip Tooltip { get; set; }
        public Highlight Highlight { get; set; }
    }
}
