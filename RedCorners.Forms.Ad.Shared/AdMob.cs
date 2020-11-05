using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

#if __IOS__
using Google.MobileAds;
using Xamarin.Forms.Platform.iOS;
#endif

#if __ANDROID__
using Android.Gms.Ads;
#endif

namespace RedCorners.Forms.Ad
{
    public class AdMob : IAd
    {
        public enum Genders : long
        {
            Unknown,
            Male,
            Female
        }

        public bool IsEnabled { get; set; } = true;
        public string BannerId { get; set; }
        public string[] TestDevices { get; set; }
        public bool IsSimulatorTestDevice { get; set; } = true;
        public string[] Keywords { get; set; }
        public DateTime? Birthday { get; set; }
        public Genders? Gender { get; set; }
        public (float Latitude, float Longitude)? Location { get; set; }

#if __ANDROID__
        class MyAdListener : AdListener
        {
            public override void OnAdClosed()
            {
                AdmobPlugin.Instance.LoadAd();
            }
        }
#endif

#if __IOS__
        Interstitial adInterstitial;
#endif
#if __ANDROID__
        InterstitialAd adInterstitial;
        AdRequest request;
#endif

        public AdMob()
        {
        }

#if __IOS__
        public void CreateAndRequestInterstitial(string interstitialId)
        {
            if (!IsEnabled)
                return;

            adInterstitial = new Interstitial(interstitialId);
            adInterstitial.ScreenDismissed += (sender, e) =>
            {
                // Interstitial is a one time use object. That means once an interstitial is shown, HasBeenUsed 
                // returns true and the interstitial can't be used to load another ad. 
                // To request another interstitial, you'll need to create a new Interstitial object.
                adInterstitial.Dispose();
                adInterstitial = null;
                CreateAndRequestInterstitial(interstitialId);
            };

            // Requests test ads on devices you specify. Your test device ID is printed to the console when
            // an ad request is made. GADBannerView automatically returns test ads when running on a
            // simulator. After you get your device ID, add it here

            var request = GetDefaultRequest();
            adInterstitial.LoadRequest(request);
        }

        public Request GetDefaultRequest()
        {
            var request = Request.GetDefaultRequest();

            var testDevices = new List<string>();
            if (IsSimulatorTestDevice)
                testDevices.Add(Request.SimulatorId.ToString());

            if (TestDevices != null)
                testDevices.AddRange(TestDevices);

            MobileAds.SharedInstance.RequestConfiguration.TestDeviceIdentifiers = testDevices.ToArray();

            if (Keywords != null)
                request.Keywords = Keywords;

            if (Birthday != null)
                request.Birthday = Birthday.Value.ToNSDate();

            if (Gender != null)
                request.Gender = (Google.MobileAds.Gender)Gender.Value;

            if (Location != null)
                request.SetLocation(new nfloat(Location.Value.Latitude), new nfloat(Location.Value.Longitude), 100);

            return request;
        }

#else
        public void CreateAndRequestInterstitial()
        {
            adInterstitial = new InterstitialAd(Forms.Context);
            adInterstitial.AdUnitId = Vars.InterstitialId;
            adInterstitial.AdListener = new MyAdListener();
            LoadAd();
        }

        public void LoadAd()
        {
            if (!IsAdEnabled)
                return;

            request = new AdRequest.Builder()
                .AddKeyword("luxembourg")
#if DEBUG
                .AddTestDevice("ED7EA56976FC3A16579281D56DB108F8")
#endif
                .Build();
            adInterstitial.LoadAd(request);
        }
#endif

        public void ShowInterstitial(Page page)
        {
            if (!IsEnabled)
                return;

#if __IOS__
            var renderer = Platform.GetRenderer(page);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(page);
                Platform.SetRenderer(page, renderer);
            }
            var viewController = renderer.ViewController;

            if (adInterstitial.IsReady)
                adInterstitial.Present(viewController);
#endif
#if __ANDROID__
            if (adInterstitial.IsLoaded)
            {
                adInterstitial.Show();
                LogSystem.Instance.TrackEvent("ShowAd");
            }
            else if (adInterstitial.IsLoading)
                LogSystem.Instance.TrackEvent("LoadingAd");
            else LogSystem.Instance.TrackEvent("IgnoreAd");
#endif
        }
    }
}