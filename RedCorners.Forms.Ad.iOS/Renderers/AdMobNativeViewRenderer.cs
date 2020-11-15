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
        IUnifiedNativeAdDelegate,
        IUnifiedNativeAdLoaderDelegate
    {
        AdLoader adLoader; 
        AdMobNativeView View => Element as AdMobNativeView;


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

            View?.TriggerAdLoading();

            Device.BeginInvokeOnMainThread(() =>
            adLoader.LoadRequest(View.AdMob.GetDefaultRequest()));
        }

        void CreateAdLoader()
        {
            if (View == null ||
                string.IsNullOrWhiteSpace(View.UnitId) ||
                View.AdMob == null)
                return;

            adLoader = new AdLoader(
                View.UnitId,
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
            View?.TriggerAdFailedToLoad((int)error.Code);
        }

        [Export("adLoader:didReceiveUnifiedNativeAd:")]
        public void DidReceiveUnifiedNativeAd(AdLoader adLoader, UnifiedNativeAd nativeAd)
        {
            Console.WriteLine("Received Ad!");
            TemplateView templateView = View.NativeTemplate == AdMobNativeTemplates.Medium ?
                (TemplateView)new MediumTemplateView(Frame) : new SmallTemplateView(Frame);
            nativeAd.Delegate = this;
            SetNativeControl(templateView);

            templateView.SetNativeAd(nativeAd);
            templateView.AddHorizontalConstraintsToSuperviewWidth();
            templateView.AddVerticalCenterConstraintToSuperview();
            View?.TriggerAdRendered();
        }

        [Export("nativeAdDidRecordImpression:")]
        public void DidRecordImpression(UnifiedNativeAd nativeAd)
        {
            View?.TriggerAdImpression();
        }

        [Export("nativeAdDidRecordClick:")]
        public void DidRecordClick(UnifiedNativeAd nativeAd)
        {
            View?.TriggerAdClicked();
        }

        [Export("nativeAdWillPresentScreen:")]
        public void WillPresentScreen(UnifiedNativeAd nativeAd)
        {
            View?.TriggerAdOpened();
        }

        [Export("nativeAdWillDismissScreen:")]
        public void WillDismissScreen(UnifiedNativeAd nativeAd)
        {

        }

        [Export("nativeAdDidDismissScreen:")]
        public void DidDismissScreen(UnifiedNativeAd nativeAd)
        {
            View?.TriggerAdClosed();
        }

        [Export("nativeAdWillLeaveApplication:")]
        public void WillLeaveApplication(UnifiedNativeAd nativeAd)
        {
            View?.TriggerAdLeftApplication();
        }

        [Export("nativeAdIsMuted:")]
        public void IsMuted(UnifiedNativeAd nativeAd)
        {

        }

        [Export("adLoaderDidFinishLoading:")]
        public void DidFinishLoading(AdLoader adLoader)
        {
            View?.TriggerAdLoaded();
        }
    }
}