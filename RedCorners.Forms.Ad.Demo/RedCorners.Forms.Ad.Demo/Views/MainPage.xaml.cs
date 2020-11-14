using RedCorners;
using RedCorners.Forms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedCorners.Forms.Ad.Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage
    {
        readonly AdMob adMob;

        public MainPage()
        {
            InitializeComponent();
            adMob = new AdMob();
#if __ANDROID__
            adMob.Context = Android.MainActivity.Context;
#endif
            adMob.CreateAndRequestInterstitial();

            AdNativeSmall.OnAdImpression += AdNativeSmall_OnAdImpression;
            AdNativeSmall.OnAdClicked += AdNativeSmall_OnAdClicked;
            AdNativeSmall.OnAdFailedToLoad += AdNativeSmall_OnAdFailedToLoad;
        }

        private void AdNativeSmall_OnAdFailedToLoad(object sender, int e)
        {
            Console.WriteLine("Failed");
        }

        private void AdNativeSmall_OnAdClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked");
        }

        private void AdNativeSmall_OnAdImpression(object sender, EventArgs e)
        {
            Console.WriteLine("Impression");
        }

        private void Btn_Interstitial_Clicked(object sender, EventArgs e)
        {
            adMob.ShowInterstitial(this);
        }
    }
}