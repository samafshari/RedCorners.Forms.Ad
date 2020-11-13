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

            public MyAdListener(AdMobNativeViewRenderer renderer)
            {
                this.renderer = renderer;
            }

            // TODO: Override Methods
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
            }
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

            if (adLoader.IsLoading)
            {
                // The AdLoader is still loading ads.
                // Expect more adLoaded or onAdFailedToLoad callbacks.
            }
            else
            {
                // The AdLoader has finished loading ads.
                var inflater = Context.GetSystemService(Context.LayoutInflaterService)
                    as LayoutInflater;

                var root = new UnifiedNativeAdView(Context);
                var nativeAdView = (UnifiedNativeAdView)
                    inflater.Inflate(Resource.Layout.gnt_medium_template_view, root);

                nativeAdView.MediaView = nativeAdView.FindViewById<MediaView>(Resource.Id.media_view);

                nativeAdView.SetNativeAd(ad);
                SetNativeControl(nativeAdView);

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
    }
}