using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RedCorners.Forms.Ad
{
    public enum AdMobNativeTemplates
    {
        Medium,
        Small
    }

    public class AdMobNativeView : View
    {
        public static readonly BindableProperty UnitIdProperty = BindableProperty.Create(
           propertyName: nameof(UnitId),
           returnType: typeof(string),
           declaringType: typeof(AdMobNativeView),
           defaultValue: AdMob.TestNativeAdvancedVideoId);

        public string UnitId
        {
            get => (string)GetValue(UnitIdProperty);
            set => SetValue(UnitIdProperty, value);
        }

        public static readonly BindableProperty AdMobProperty = BindableProperty.Create(
           propertyName: nameof(AdMob),
           returnType: typeof(AdMob),
           declaringType: typeof(AdMobNativeView),
           defaultValue: new AdMob());

        public AdMob AdMob
        {
            get => (AdMob)GetValue(AdMobProperty);
            set => SetValue(AdMobProperty, value);
        }

        public static readonly BindableProperty NativeTemplateProperty = BindableProperty.Create(
           propertyName: nameof(NativeTemplate),
           returnType: typeof(AdMobNativeTemplates),
           declaringType: typeof(AdMobNativeView),
           defaultValue: AdMobNativeTemplates.Medium);

        public AdMobNativeTemplates NativeTemplate
        {
            get => (AdMobNativeTemplates)GetValue(NativeTemplateProperty);
            set => SetValue(NativeTemplateProperty, value);
        }
    }
}
