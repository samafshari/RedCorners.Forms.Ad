using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

#if __ANDROID__
using Xamarin.Forms.Platform.Android;
#else
using Xamarin.Forms.Platform.iOS;
using Google.MobileAds;
using CoreGraphics;
using UIKit;
#endif

[assembly: ExportRenderer(typeof(RedCorners.Forms.Ad.AdMobBannerView), typeof(RedCorners.Forms.Ad.Renderers.AdMobBannerViewRenderer))]
namespace RedCorners.Forms.Ad
{
    public enum AdMobBannerSizes
    {
        LargeBanner,
        MediumRectangle
    }

    public class AdMobBannerView : View
    {
        public static readonly BindableProperty BannerSizeProperty = BindableProperty.Create(
           propertyName: nameof(BannerSize),
           returnType: typeof(AdMobBannerSizes),
           declaringType: typeof(AdMobBannerView),
           defaultValue: AdMobBannerSizes.LargeBanner);

        public AdMobBannerSizes BannerSize
        {
            get => (AdMobBannerSizes)GetValue(BannerSizeProperty);
            set => SetValue(BannerSizeProperty, value);
        }
        
        public static readonly BindableProperty UnitIdProperty = BindableProperty.Create(
           propertyName: nameof(UnitId),
           returnType: typeof(string),
           declaringType: typeof(AdMobBannerView));

        public string UnitId
        {
            get => (string)GetValue(UnitIdProperty);
            set => SetValue(UnitIdProperty, value);
        }
        
        public static readonly BindableProperty AdMobProperty = BindableProperty.Create(
           propertyName: nameof(AdMob),
           returnType: typeof(AdMob),
           declaringType: typeof(AdMobBannerView),
           defaultValue: new AdMob());

        public AdMob AdMob
        {
            get => (AdMob)GetValue(AdMobProperty);
            set => SetValue(AdMobProperty, value);
        }
    }

    namespace Renderers
    {
        public class AdMobBannerViewRenderer : ViewRenderer
        {
            public AdMobBannerViewRenderer() { }

#if __ANDROID__
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                var adsbanner = (BannerAd)Element;
                //var adview = new Android.Gms.Ads.NativeExpressAdView(Context);
                //adview.AdUnitId = Constants.NativeAdId;
                //adview.AdSize = AdSize.SmartBanner;
                var adview = new AdView(Context);
        if (adsbanner.AdSize == BannerAdSizes.LargeBanner)
                adview.AdSize = AdSize.LargeBanner;
        else if (adsbanner.AdSize == BannerAdSizes.MediumRectangle)
        adview.AdSize = AdSize.MediumRectangle;
                adview.AdUnitId = Vars.BannerId;

                base.SetNativeControl(adview);

                var requestbuilder = new AdRequest.Builder()
#if DEBUG
                    .AddTestDevice("ED7EA56976FC3A16579281D56DB108F8")
                    .AddTestDevice("8EBFBD0CEA9400AE134EB85743BD37E2")
#endif
                    .AddKeyword("luxembourg");
                adview.LoadAd(requestbuilder.Build());
                adview.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
#elif __IOS__
            BannerView bannerView;
            bool viewOnScreen = false;

            protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
            {
                base.OnElementChanged(e);
                if (e.NewElement == null) return;
                if (e.OldElement == null)
                {
                    UIViewController viewCtrl = null;

                    foreach (UIWindow v in UIApplication.SharedApplication.Windows)
                    {
                        if (v.RootViewController != null)
                        {
                            viewCtrl = v.RootViewController;
                        }
                    }

                    var view = (AdMobBannerView)Element;
                    view.PropertyChanged += View_PropertyChanged;
                    var size = AdSizeCons.LargeBanner;
                    if (view.BannerSize == AdMobBannerSizes.MediumRectangle)
                        size = AdSizeCons.MediumRectangle;
                    bannerView = new BannerView(size: AdSizeCons.LargeBanner, origin: new CGPoint(0, 0))
                    {
                        AdUnitId = view.UnitId,
                        RootViewController = viewCtrl
                    };

                    bannerView.AdReceived += (sender, args) =>
                    {
                        if (!viewOnScreen)
                            this.AddSubview(bannerView);
                        viewOnScreen = true;
                    };
                    bannerView.ReceiveAdFailed += (sender, args) =>
                    {
                    };
                    if (!string.IsNullOrWhiteSpace(bannerView.AdUnitId))
                        LoadRequest();
                    base.SetNativeControl(bannerView);
                }
            }

            private void View_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                var view = Element as AdMobBannerView;
                if (e.PropertyName == nameof(AdMobBannerView.UnitId))
                {
                    bannerView.AdUnitId = view.UnitId;
                    LoadRequest();
                }
            }

            public override void MovedToWindow()
            {
                base.MovedToWindow();
                if (bannerView == null) return;
                if (bannerView.RootViewController != null) return;
                bannerView.RootViewController = UIApplication.SharedApplication.Windows[0].RootViewController;
            }

            void LoadRequest()
            {
                var view = Element as AdMobBannerView;
                if (view == null) return;
                var adMob = view.AdMob;
                if (adMob == null) return;

                var request = adMob.GetDefaultRequest(); 
                bannerView.LoadRequest(request);
            }
#endif
        }
    }
}
