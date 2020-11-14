using Foundation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;
using CoreGraphics;
using Google.MobileAds;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace RedCorners.Forms.Ad.iOS
{
    public class TemplateView : UnifiedNativeAdView
    {
        const string StyleKeyCallToActionFont = @"call_to_action_font";
        const string StyleKeyCallToActionFontColor = @"call_to_action_font_color";
        const string StyleKeyCallToActionBackgroundColor = @"call_to_action_background_color";
        const string StyleKeySecondaryFont = @"secondary_font";
        const string StyleKeySecondaryFontColor = @"secondary_font_color";
        const string StyleKeySecondaryBackgroundColor = @"secondary_background_color";
        const string StyleKeyPrimaryFont = @"primary_font";
        const string StyleKeyPrimaryFontColor = @"primary_font_color";
        const string StyleKeyPrimaryBackgroundColor = @"primary_background_color";
        const string StyleKeyTertiaryFont = @"tertiary_font";
        const string StyleKeyTertiaryFontColor = @"tertiary_font_color";
        const string StyleKeyTertiaryBackgroundColor = @"tertiary_background_color";
        const string StyleKeyMainBackgroundColor = @"main_background_color";
        const string StyleKeyCornerRadius = @"corner_radius";

        const string Blue = "#5C84F0";

        Dictionary<string, NSObject> styles;
        
        UILabel
            primaryTextView,
            secondaryTextView,
            tertiaryTextView,
            adBadge;

        UIImageView brandImage;
        UIView backgroundView;
        UIView rootView;

        public TemplateView(CGRect frame, string nibName) : base(frame)
        {
            rootView = NSBundle.MainBundle.LoadNib(nibName, this, null)
                .GetItem<UIView>(0);

            AddSubview(rootView);
            var dic = NSDictionary.FromObjectAndKey(rootView, new NSString("_rootView"));
            AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[_rootView]|", 0, null, dic));
            AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[_rootView]|", 0, null, dic));

            ApplyStyles();
            StyleAdBadge();
        }

        NSObject GetStyleForKey(string key)
        {
            if (styles != null && styles.TryGetValue(key, out var val))
                    return val;
            return null;
        }

        public bool HasStyleForKey(string key) => GetStyleForKey(key) != null;

        void ApplyStyles()
        {
            Layer.BorderColor = Color.FromHex("#E0E0E0").ToCGColor();
            Layer.BorderWidth = 1.0f;
            MediaView.SizeToFit();

            if (HasStyleForKey(StyleKeyCornerRadius))
            {
                var roundedCornerRadius = ((NSNumber)GetStyleForKey(StyleKeyCornerRadius)).FloatValue;

                // rounded corners
                IconView.Layer.CornerRadius = roundedCornerRadius;
                IconView.ClipsToBounds = true;

                ((UIButton)CallToActionView).Layer.CornerRadius = roundedCornerRadius;
                ((UIButton)CallToActionView).ClipsToBounds = true;
            }

            // Fonts
            if (HasStyleForKey(StyleKeyPrimaryFont))
                primaryTextView.Font = (UIFont)GetStyleForKey(StyleKeyPrimaryFont);
            if (HasStyleForKey(StyleKeySecondaryFont))
                secondaryTextView.Font = (UIFont)GetStyleForKey(StyleKeySecondaryFont);
            if (HasStyleForKey(StyleKeyTertiaryFont))
                tertiaryTextView.Font = (UIFont)GetStyleForKey(StyleKeyTertiaryFont);
            if (HasStyleForKey(StyleKeyCallToActionFont))
                ((UIButton)CallToActionView).TitleLabel.Font = (UIFont)GetStyleForKey(StyleKeyCallToActionFont);

            // Font Colors
            if (HasStyleForKey(StyleKeyPrimaryFontColor))
                primaryTextView.TextColor = (UIColor)GetStyleForKey(StyleKeyPrimaryFontColor);
            if (HasStyleForKey(StyleKeySecondaryFontColor))
                secondaryTextView.TextColor = (UIColor)GetStyleForKey(StyleKeySecondaryFontColor);
            if (HasStyleForKey(StyleKeyTertiaryFontColor))
                tertiaryTextView.TextColor = (UIColor)GetStyleForKey(StyleKeyTertiaryFontColor);
            if (HasStyleForKey(StyleKeyCallToActionFontColor))
                ((UIButton)CallToActionView).SetTitleColor((UIColor)GetStyleForKey(StyleKeyCallToActionFontColor), UIControlState.Normal);

            // Background Colors
            if (HasStyleForKey(StyleKeyPrimaryBackgroundColor))
                primaryTextView.BackgroundColor = (UIColor)GetStyleForKey(StyleKeyPrimaryBackgroundColor);
            if (HasStyleForKey(StyleKeySecondaryBackgroundColor))
                secondaryTextView.BackgroundColor = (UIColor)GetStyleForKey(StyleKeySecondaryBackgroundColor);
            if (HasStyleForKey(StyleKeyTertiaryBackgroundColor))
                tertiaryTextView.BackgroundColor = (UIColor)GetStyleForKey(StyleKeyTertiaryBackgroundColor);
            if (HasStyleForKey(StyleKeyCallToActionBackgroundColor))
                ((UIButton)CallToActionView).BackgroundColor = (UIColor)GetStyleForKey(StyleKeyCallToActionBackgroundColor);
            if (HasStyleForKey(StyleKeyMainBackgroundColor))
                BackgroundColor = (UIColor)GetStyleForKey(StyleKeyMainBackgroundColor);
            if (backgroundView != null && HasStyleForKey(StyleKeyPrimaryBackgroundColor))
                backgroundView.BackgroundColor = (UIColor)GetStyleForKey(StyleKeyPrimaryBackgroundColor);
        }

        void StyleAdBadge()
        {
            adBadge.Layer.BorderColor = adBadge.TextColor.CGColor;
            adBadge.Layer.BorderWidth = 1.0f;
            adBadge.Layer.CornerRadius = 3.0f;
        }

        void SetStyles(Dictionary<string, NSObject> styles)
        {
            this.styles = styles;
            ApplyStyles();
        }

        public void SetNativeAd(UnifiedNativeAd nativeAd)
        {
            base.NativeAd = nativeAd;
            HeadlineView = primaryTextView;
            var adBody = nativeAd.Body;
            var cta = nativeAd.CallToAction;
            var headline = nativeAd.Headline;
            string tertiaryText = string.Empty;

            if (!string.IsNullOrWhiteSpace(nativeAd.Store) && string.IsNullOrWhiteSpace(nativeAd.Advertiser))
            {
                // Ad has store but not advertiser
                StoreView = tertiaryTextView;
                tertiaryText = nativeAd.Store;
            }

            // WHAT AN IDIOT who wrote the original obj-c code.
            else if (string.IsNullOrWhiteSpace(nativeAd.Store) && !string.IsNullOrWhiteSpace(nativeAd.Advertiser))
            {
                // Ad has advertiser but not store
                AdvertiserView = tertiaryTextView;
                tertiaryText = nativeAd.Advertiser;
            }
            else if (string.IsNullOrWhiteSpace(nativeAd.Store) && string.IsNullOrWhiteSpace(nativeAd.Advertiser))
            {
                // Ad has both store and advertiser, default to showing advertiser.
                AdvertiserView = tertiaryTextView;
                tertiaryText = nativeAd.Advertiser;
            }

            primaryTextView.Text = headline;
            tertiaryTextView.Text = tertiaryText;
            ((UIButton)CallToActionView).SetTitle(cta, UIControlState.Normal);

            // Body text
            // We either show the number of stars an app has, or show the body of the ad.
            // Use the unicode characters for filled in or empty stars.
            if (nativeAd.StarRating.FloatValue > 0)
            {
                var stars = "";
                int count = 0;
                for (; count < nativeAd.StarRating.Int32Value; count++)
                    stars += "\u2605"; // filled star
                for (; count < 5; count++)
                    stars += "\u2606"; // empty star
                adBody = stars;
                StarRatingView = secondaryTextView;
            }
            else BodyView = secondaryTextView;

            secondaryTextView.Text = adBody;

            if (nativeAd.Icon != null)
                ((UIImageView)IconView).Image = nativeAd.Icon.Image;

            MediaView.MediaContent = nativeAd.MediaContent;
        }

        void AddHorizontalConstraintsToSuperviewWidth()
        {
            if (base.Superview == null) return;

            UIView child = this;
            Superview.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[child]|", 0, null,
                NSDictionary.FromObjectAndKey(child, new NSString("child"))));
        }

        void AddVerticalCenterConstraintToSuperview()
        {
            if (base.Superview == null) return;

            var child = this;
            Superview.AddConstraint(NSLayoutConstraint.Create(
                Superview,
                NSLayoutAttribute.CenterY,
                NSLayoutRelation.Equal,
                child,
                NSLayoutAttribute.CenterY,
                multiplier: 1,
                constant: 0));
        }
    }
}