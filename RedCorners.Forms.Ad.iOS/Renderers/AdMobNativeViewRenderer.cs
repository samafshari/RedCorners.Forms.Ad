using Foundation;

using Google.MobileAds;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RedCorners.Forms.Ad.AdMobNativeView), typeof(RedCorners.Forms.Ad.iOS.Renderers.AdMobNativeViewRenderer))]
namespace RedCorners.Forms.Ad.iOS.Renderers
{
    public class AdMobNativeViewRenderer : ViewRenderer,
        //IAdLoaderDelegate
        IUnifiedNativeAdLoaderDelegate
    {
        AdLoader adLoader;

        public AdMobNativeViewRenderer() { }

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
            Device.BeginInvokeOnMainThread(() =>
            adLoader.LoadRequest(view.AdMob.GetDefaultRequest()));
        }

        void CreateAdLoader()
        {
            var view = Element as AdMobNativeView;
            if (view == null ||
                string.IsNullOrWhiteSpace(view.UnitId) ||
                view.AdMob == null)
                return;

            adLoader = new AdLoader(
                view.UnitId,
                ViewController,
                new[] { AdLoaderAdType.UnifiedNative },
                new[] { new AdLoaderOptions() });
            adLoader.Delegate = this;
        }

        private void NewElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AdMobNativeView.AdMob) ||
                e.PropertyName == nameof(AdMobNativeView.UnitId))
                LoadAds();
        }

        public void DidFailToReceiveAd(AdLoader adLoader, RequestError error)
        {
            Console.WriteLine("Failed");
        }

        [Export("adLoader:didReceiveUnifiedNativeAd:")]
        public void DidReceiveUnifiedNativeAd(AdLoader adLoader, UnifiedNativeAd nativeAd)
        {
            Console.WriteLine("Received Ad!");
        }

        public void DidRecordImpression(UnifiedNativeAd nativeAd)
        {
            Console.WriteLine("DidRecordImpression");
        }

    }
}