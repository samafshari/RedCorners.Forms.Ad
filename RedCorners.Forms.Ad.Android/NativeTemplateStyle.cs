using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedCorners.Forms.Ad.Android
{
    internal class NativeTemplateStyle
    {
        public Typeface CallToActionTextTypeface { get; set; }
        public float CallToActionTextSize { get; set; }
        public int CallToActionTypefaceColor { get; set; }
        public ColorDrawable CallToActionBackgroundColor { get; set; }
        public Typeface PrimaryTextTypeface { get; set; }
        public float PrimaryTextSize { get; set; }
        public int PrimaryTextTypefaceColor { get; set; }
        public ColorDrawable PrimaryTextBackgroundColor { get; set; }
        public Typeface SecondaryTextTypeface { get; set; }
        public float SecondaryTextSize { get; set; }
        public int SecondaryTextTypefaceColor { get; set; }
        public ColorDrawable SecondaryTextBackgroundColor { get; set; }
        public Typeface TertiaryTextTypeface { get; set; }
        public float TertiaryTextSize { get; set; }
        public int TertiaryTextTypefaceColor { get; set; }
        public ColorDrawable TertiaryTextBackgroundColor { get; set; }
        public ColorDrawable MainBackgroundColor { get; set; }
    }
}