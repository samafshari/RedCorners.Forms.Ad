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
using global::Android.Content;
using global::Android.Gms.Ads;
#endif

namespace RedCorners.Forms.Ad
{
    public class AdMob : AdBase
    {
        public enum Genders : long
        {
            Unknown,
            Male,
            Female
        }

        public bool IsEnabled { get; set; } = true;
        public string[] TestDevices { get; set; }
        public bool IsSimulatorTestDevice { get; set; } = true;
        public string[] Keywords { get; set; }
        public DateTime? Birthday { get; set; }
        public Genders? Gender { get; set; }
        public (float Latitude, float Longitude)? Location { get; set; }

#if __ANDROID__
        public Context Context { get; set; }

        class MyAdListener : AdListener
        {
            readonly AdMob adMob;

            public MyAdListener(AdMob adMob)
            {
                this.adMob = adMob;
            }

            public override void OnAdClosed()
            {
                adMob.LoadAd();
            }
        }

        InterstitialAd adInterstitial;
        AdRequest request;
        public AdMob(Context context = null)
        {
            this.Context = context;
        }

        public void CreateAndRequestInterstitial(string interstitialId)
        {
            adInterstitial = new InterstitialAd(Context);
            adInterstitial.AdUnitId = interstitialId;
            adInterstitial.AdListener = new MyAdListener(this);
            LoadAd();
        }

        public void LoadAd()
        {
            if (!IsEnabled)
                return;

            request = GetDefaultRequest();
            adInterstitial.LoadAd(request);
        }

        public AdRequest GetDefaultRequest()
        {
            var request = new AdRequest.Builder();

            var testDevices = MobileAds.RequestConfiguration.TestDeviceIds;
            if (IsSimulatorTestDevice)
                testDevices.Add(AdRequest.DeviceIdEmulator);

            if (TestDevices != null)
                foreach (var device in TestDevices)
                    testDevices.Add(device);

            if (Keywords != null)
                foreach (var keyword in Keywords)
                    request.AddKeyword(keyword);

            if (Birthday != null)
            {
                request.SetBirthday(new Java.Util.Date(
                    Birthday.Value.Year,
                    Birthday.Value.Month,
                    Birthday.Value.Day));
            }

            if (Gender != null)
                request.SetGender((int)Gender.Value);

            if (Location != null)
            {
                var l = new global::Android.Locations.Location("x");
                l.Latitude = Location.Value.Latitude;
                l.Longitude = Location.Value.Longitude;
                request.SetLocation(l);
            }

            return request.Build();
        }
#elif __IOS__
        Interstitial adInterstitial;
        
        public AdMob()
        {
        }

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
#elif __ANDROID__
            if (adInterstitial.IsLoaded)
            {
                adInterstitial.Show();
            }
#endif
        }
    }
}