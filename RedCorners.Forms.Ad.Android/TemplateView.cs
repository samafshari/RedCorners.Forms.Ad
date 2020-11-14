using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Gms.Ads.Formats;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;


using AndroidX.ConstraintLayout.Widget;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedCorners.Forms.Ad.Android
{
    internal class TemplateView : FrameLayout
    {
        int templateType;
        NativeTemplateStyle styles;
        UnifiedNativeAd nativeAd;
        UnifiedNativeAdView nativeAdView;

        public UnifiedNativeAdView NativeAdView => nativeAdView;

        TextView
            primaryView,
            secondaryView,
            tertiaryView;

        RatingBar
            ratingBar;

        ImageView
            iconView;

        MediaView
            mediaView;

        Button
            callToActionView;

        ConstraintLayout
            background;

        public const string MediumTemplate = "medium_template";
        public const string SmallTemplate = "small_template";

        public TemplateView(Context context) : base(context)
        {
            //InitView(context, null);
        }

        public TemplateView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitView(context, attrs);
        }

        public TemplateView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitView(context, attrs);
        }

        public TemplateView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) :
            base(context, attrs, defStyleAttr, defStyleRes)
        {
            InitView(context, attrs);
        }

        public View InitView(Context context, IAttributeSet attributeSet, AdMobNativeTemplates template = AdMobNativeTemplates.Medium)
        {
            //TypedArray attributes =
            //    context.Theme.ObtainStyledAttributes(attributeSet, Resource.Styleable.TemplateView, 0, 0);

            //try
            //{
            //    templateType = attributes.GetResourceId(
            //        Resource.Styleable.TemplateView_gnt_template_type,
            //        Resource.Layout.gnt_medium_template_view); // TODO: Flag to use small
            //}
            //finally
            //{
            //    attributes.Recycle();
            //}

            if (template == AdMobNativeTemplates.Medium)
                templateType = Resource.Layout.gnt_medium_template_view;
            else
                templateType = Resource.Layout.gnt_small_template_view;
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            return inflater.Inflate(templateType, this);
        }

        public void SetStyles(NativeTemplateStyle styles)
        {
            this.styles = styles;
            ApplyStyles();
        }

        void ApplyStyles()
        {
            if (styles.MainBackgroundColor != null)
            {
                background.Background = styles.MainBackgroundColor;
                if (primaryView != null)
                    primaryView.Background = styles.MainBackgroundColor;
                if (secondaryView != null)
                    secondaryView.Background = styles.MainBackgroundColor;
                if (tertiaryView != null)
                    tertiaryView.Background = styles.MainBackgroundColor;
            }

            if (primaryView != null)
            {
                if (styles.PrimaryTextTypeface != null)
                    primaryView.Typeface = styles.PrimaryTextTypeface;
                if (styles.PrimaryTextTypefaceColor > 0)
                    primaryView.SetTextColor(new Color(styles.PrimaryTextTypefaceColor));
                if (styles.PrimaryTextSize > 0)
                    primaryView.SetTextSize(ComplexUnitType.Dip, styles.PrimaryTextSize);
                if (styles.PrimaryTextBackgroundColor != null)
                    primaryView.Background = styles.PrimaryTextBackgroundColor;
            }
            
            if (secondaryView != null)
            {
                if (styles.SecondaryTextTypeface != null)
                    secondaryView.Typeface = styles.SecondaryTextTypeface;
                if (styles.SecondaryTextTypefaceColor > 0)
                    secondaryView.SetTextColor(new Color(styles.SecondaryTextTypefaceColor));
                if (styles.SecondaryTextSize > 0)
                    secondaryView.SetTextSize(ComplexUnitType.Dip, styles.SecondaryTextSize);
                if (styles.SecondaryTextBackgroundColor != null)
                    secondaryView.Background = styles.SecondaryTextBackgroundColor;
            }
            
            if (tertiaryView != null)
            {
                if (styles.TertiaryTextTypeface != null)
                    tertiaryView.Typeface = styles.TertiaryTextTypeface;
                if (styles.TertiaryTextTypefaceColor > 0)
                    tertiaryView.SetTextColor(new Color(styles.TertiaryTextTypefaceColor));
                if (styles.TertiaryTextSize > 0)
                    tertiaryView.SetTextSize(ComplexUnitType.Dip, styles.TertiaryTextSize);
                if (styles.TertiaryTextBackgroundColor != null)
                    tertiaryView.Background = styles.TertiaryTextBackgroundColor;
            }
            
            if (callToActionView != null)
            {
                if (styles.CallToActionTextTypeface != null)
                    callToActionView.Typeface = styles.CallToActionTextTypeface;
                if (styles.CallToActionTypefaceColor > 0)
                    callToActionView.SetTextColor(new Color(styles.CallToActionTypefaceColor));
                if (styles.CallToActionTextSize > 0)
                    callToActionView.SetTextSize(ComplexUnitType.Dip, styles.CallToActionTextSize);
                if (styles.CallToActionBackgroundColor != null)
                    callToActionView.Background = styles.CallToActionBackgroundColor;
            }

            Invalidate();
            RequestLayout();
        }

        bool AdHasOnlyStore(UnifiedNativeAd nativeAd) =>
            !string.IsNullOrWhiteSpace(nativeAd.Store) &&
            string.IsNullOrWhiteSpace(nativeAd.Advertiser);

        public void SetNativeAd(UnifiedNativeAd nativeAd)
        {
            PostInflate();

            this.nativeAd = nativeAd;

            nativeAdView.CallToActionView = callToActionView;
            nativeAdView.HeadlineView = primaryView;
            nativeAdView.MediaView = mediaView;

            string secondaryText = "";
            secondaryView.Visibility = ViewStates.Visible;
            if (AdHasOnlyStore(nativeAd))
            {
                nativeAdView.StoreView = secondaryView;
                secondaryText = nativeAd.Store;
            }
            else if (!string.IsNullOrWhiteSpace(nativeAd.Advertiser))
            {
                nativeAdView.AdvertiserView = secondaryView;
                secondaryText = nativeAd.Advertiser;
            }

            primaryView.Text = nativeAd.Headline;
            callToActionView.Text = nativeAd.CallToAction;

            if (nativeAd.StarRating != null && nativeAd.StarRating.DoubleValue() > 0.0)
            {
                secondaryView.Visibility = ViewStates.Gone;
                ratingBar.Visibility = ViewStates.Visible;
                ratingBar.Max = 5;
                nativeAdView.StarRatingView = ratingBar;
            }
            else
            {
                secondaryView.Text = secondaryText;
                secondaryView.Visibility = ViewStates.Visible;
                ratingBar.Visibility = ViewStates.Gone;
            }

            if (nativeAd.Icon != null)
            {
                iconView.Visibility = ViewStates.Visible;
                iconView.SetImageDrawable(nativeAd.Icon.Drawable);
            }
            else
            {
                iconView.Visibility = ViewStates.Gone;
            }

            if (tertiaryView != null && !string.IsNullOrWhiteSpace(nativeAd.Body))
            {
                tertiaryView.Visibility = ViewStates.Visible;
                tertiaryView.Text = nativeAd.Body;
                nativeAdView.BodyView = tertiaryView;
            }
            else if (tertiaryView != null)
            {
                tertiaryView.Visibility = ViewStates.Gone;
            }

            nativeAdView.SetNativeAd(nativeAd);
        }

        public void DestroyNativeAd()
        {
            nativeAd.Destroy();
        }

        public string GetTemplateTypeName()
        {
            if (templateType == Resource.Layout.gnt_medium_template_view)
                return MediumTemplate;
            else if (templateType == Resource.Layout.gnt_small_template_view)
                return SmallTemplate;

            return "";
        }

        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();
            PostInflate();
        }

        void PostInflate()
        {
            nativeAdView = FindViewById<UnifiedNativeAdView>(Resource.Id.native_ad_view);
            primaryView = FindViewById<TextView>(Resource.Id.primary);
            secondaryView = FindViewById<TextView>(Resource.Id.secondary);
            tertiaryView = FindViewById<TextView>(Resource.Id.body);

            ratingBar = FindViewById<RatingBar>(Resource.Id.rating_bar);
            ratingBar.Enabled = false;

            callToActionView = FindViewById<Button>(Resource.Id.cta);
            iconView = FindViewById<ImageView>(Resource.Id.icon);
            mediaView = FindViewById<MediaView>(Resource.Id.media_view);
            background = FindViewById<ConstraintLayout>(Resource.Id.background);
        }
    }
}