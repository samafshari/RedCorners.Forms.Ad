using RedCorners;
using RedCorners.Components;
using RedCorners.Forms;
using RedCorners.Forms.Ad.Demo.Systems;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RedCorners.Forms.Ad.Demo
{
    public partial class App : Application2
    {
        //App Open    ca-app-pub-3940256099942544/3419835294
        //Banner ca-app-pub-3940256099942544/6300978111
        //Interstitial ca-app-pub-3940256099942544/1033173712
        //Interstitial Video  ca-app-pub-3940256099942544/8691691433
        //Rewarded Video  ca-app-pub-3940256099942544/5224354917
        //Native Advanced ca-app-pub-3940256099942544/2247696110
        //Native Advanced Video ca-app-pub-3940256099942544/1044960115

        public static string InterstitialId = "ca-app-pub-3940256099942544/1033173712";
        public static string BannerId = "ca-app-pub-3940256099942544/6300978111";

        public override Page GetFirstPage()
            => new Views.MainPage();

        public override void InitializeSystems()
        {
            InitializeComponent();
            base.InitializeSystems();
            SplashTasks.Add(SettingsSystem.Instance.InitializeAsync);
        }
    }
}
