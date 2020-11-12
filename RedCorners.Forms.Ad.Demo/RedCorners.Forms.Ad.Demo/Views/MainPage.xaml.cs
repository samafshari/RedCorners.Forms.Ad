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
            adMob.CreateAndRequestInterstitial(App.InterstitialId);
        }

        private void Btn_Interstitial_Clicked(object sender, EventArgs e)
        {
            adMob.ShowInterstitial(this);
        }
    }
}