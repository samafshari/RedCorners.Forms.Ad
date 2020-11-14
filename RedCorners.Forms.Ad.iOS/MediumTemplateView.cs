using CoreGraphics;

using Foundation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;

namespace RedCorners.Forms.Ad.iOS
{
    public class MediumTemplateView : TemplateView
    {
        public MediumTemplateView(CGRect frame) :
            base(frame, "GADTMediumTemplateView")
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
        }
    }
}