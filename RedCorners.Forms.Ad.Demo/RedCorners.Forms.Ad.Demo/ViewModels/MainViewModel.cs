using RedCorners;
using RedCorners.Forms;
using RedCorners.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RedCorners.Forms.Ad.Demo.ViewModels
{
    public class MainViewModel : BindableModel
    {
        public Action AdClickedAction => () => 
            Console.WriteLine("Clicked");
        public Action AdClosedAction => () => 
            Console.WriteLine("Closed");
        public Action AdImpressionAction => () => 
            Console.WriteLine("Impression");
    }
}