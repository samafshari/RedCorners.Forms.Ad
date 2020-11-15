using CoreGraphics;

using Foundation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;

namespace RedCorners.Forms.Ad.iOS
{
    [Register("GADTSmallTemplateView")]
    public class SmallTemplateView : TemplateView
    {
        public SmallTemplateView(CGRect frame) :
            base(frame, "GADTSmallTemplateView")
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
        }
    }
}