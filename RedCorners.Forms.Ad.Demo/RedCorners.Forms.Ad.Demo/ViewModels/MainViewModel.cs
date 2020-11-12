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
        public override bool IsModal => false;

        public MainViewModel()
        {
            Status = TaskStatuses.Success;
            Count = Settings.Instance.Count;
        }

        public int Count { get; set; }

        public Command CountCommand => new Command(() =>
        {
            // Increase the count
            Count++;
            UpdateProperties();

            // Store it in settings and save
            Settings.Instance.Count = Count;
            Signals.SaveSettings.Signal();
        });
    }
}