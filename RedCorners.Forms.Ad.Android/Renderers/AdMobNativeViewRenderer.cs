using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using global::Android.Content;
using global::Android.Gms.Ads;
using global::Android.Gms.Ads.Formats;
using global::Android.Views;
using View = Xamarin.Forms.View;
using AView = Android.Views.View;
using Resource = RedCorners.Forms.Ad.Android.Resource;

[assembly: ExportRenderer(typeof(RedCorners.Forms.Ad.AdMobNativeView), typeof(RedCorners.Forms.Ad.Android.Renderers.AdMobNativeViewRenderer))]
namespace RedCorners.Forms.Ad.Android.Renderers
{
    public class AdMobNativeViewRenderer : ViewRenderer,
        UnifiedNativeAd.IOnUnifiedNativeAdLoadedListener
    {
        class MyAdListener : AdListener
        {
            readonly AdMobNativeViewRenderer renderer;
            AdMobNativeView View => renderer.Element as AdMobNativeView;

            public MyAdListener(AdMobNativeViewRenderer renderer)
            {
                this.renderer = renderer;
            }

            public override void OnAdClicked()
            {
                base.OnAdClicked();
                View?.TriggerAdClicked();
            }

            public override void OnAdClosed()
            {
                base.OnAdClosed();
                View?.TriggerAdClosed();
            }

            public override void OnAdImpression()
            {
                base.OnAdImpression();
                View?.TriggerAdImpression();
            }

            public override void OnAdFailedToLoad(int p0)
            {
                base.OnAdFailedToLoad(p0);
                View?.TriggerAdFailedToLoad(p0);
            }

            public override void OnAdOpened()
            {
                base.OnAdOpened();
                View?.TriggerAdOpened();
            }

            public override void OnAdLeftApplication()
            {
                base.OnAdLeftApplication();
                View?.TriggerAdLeftApplication();
            }

            public override void OnAdLoaded()
            {
                base.OnAdLoaded();
                View?.TriggerAdLoaded();
            }
        }

        AdLoader adLoader;

        public AdMobNativeViewRenderer(Context context) : base(context)
        {

        }

        [Obsolete]
        public AdMobNativeViewRenderer()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                LoadAds();
                e.NewElement.PropertyChanged += NewElement_PropertyChanged;
            }
            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= NewElement_PropertyChanged;
            }
        }

        void LoadAds()
        {
            if (adLoader == null)
                CreateAdLoader();

            if (adLoader == null)
                return;

            var view = Element as AdMobNativeView;
            adLoader.LoadAd(view.AdMob.GetDefaultRequest());
        }

        private void NewElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AdMobNativeView.AdMob) ||
                e.PropertyName == nameof(AdMobNativeView.UnitId))
                LoadAds();
        }

        void CreateAdLoader()
        {
            var view = Element as AdMobNativeView;
            if (view == null ||
                string.IsNullOrWhiteSpace(view.UnitId) ||
                view.AdMob == null)
                return;

            var nativeAdOptionsBuilder = new NativeAdOptions.Builder();

            var adLoaderBuilder = new AdLoader.Builder(Context, view.UnitId);
            adLoaderBuilder
                .ForUnifiedNativeAd(this)
                .WithAdListener(new MyAdListener(this))
                .WithNativeAdOptions(nativeAdOptionsBuilder.Build());

            adLoader = adLoaderBuilder.Build();
        }

        public void OnUnifiedNativeAdLoaded(UnifiedNativeAd ad)
        {
            // TODO: Show the ad.
            var view = Element as AdMobNativeView;

            if (adLoader.IsLoading)
            {
                // The AdLoader is still loading ads.
                // Expect more adLoaded or onAdFailedToLoad callbacks.
                view.TriggerAdLoading();
            }
            else
            {
                // The AdLoader has finished loading ads.
                var styles = new NativeTemplateStyle
                {
                };

                var template = new TemplateView(Context);
                var aview = template.InitView(Context, null, view.NativeTemplate);
                SetNativeControl(aview);
                template.SetStyles(styles);
                template.SetNativeAd(ad);
                view.TriggerAdRendered();
                //var inflater = Context.GetSystemService(Context.LayoutInflaterService)
                //    as LayoutInflater;
                //
                //var root = new UnifiedNativeAdView(Context);
                //var nativeAdView = (UnifiedNativeAdView)
                //    inflater.Inflate(Resource.Layout.gnt_medium_template_view, root);
                //
                //nativeAdView.MediaView = nativeAdView.FindViewById<MediaView>(Resource.Id.media_view);
                //
                //nativeAdView.SetNativeAd(ad);

            }
        }
    }
}