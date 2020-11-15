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
        public event EventHandler OnAdClicked; 
        public event EventHandler OnAdClosed;
        public event EventHandler OnAdImpression; 
        public event EventHandler<int> OnAdFailedToLoad; 
        public event EventHandler OnAdOpened; 
        public event EventHandler OnAdLeftApplication; 
        public event EventHandler OnAdLoaded; 
        public event EventHandler OnAdRendered; 
        public event EventHandler OnAdLoading;

        public Action AdClickedAction
        {
            get => (Action)GetValue(AdClickedActionProperty);
            set => SetValue(AdClickedActionProperty, value);
        }

        public static readonly BindableProperty AdClickedActionProperty = BindableProperty.Create(
            nameof(AdClickedAction),
            typeof(Action),
            typeof(AdMobNativeView));
        
        public Action AdClosedAction
        {
            get => (Action)GetValue(AdClosedActionProperty);
            set => SetValue(AdClosedActionProperty, value);
        }

        public static readonly BindableProperty AdClosedActionProperty = BindableProperty.Create(
            nameof(AdClosedAction),
            typeof(Action),
            typeof(AdMobNativeView));
        
        public Action AdImpressionAction
        {
            get => (Action)GetValue(AdImpressionActionProperty);
            set => SetValue(AdImpressionActionProperty, value);
        }

        public static readonly BindableProperty AdImpressionActionProperty = BindableProperty.Create(
            nameof(AdImpressionAction),
            typeof(Action),
            typeof(AdMobNativeView));
        
        public Action<int> AdFailedToLoadAction
        {
            get => (Action<int>)GetValue(AdFailedToLoadProperty);
            set => SetValue(AdFailedToLoadProperty, value);
        }

        public static readonly BindableProperty AdFailedToLoadProperty = BindableProperty.Create(
            nameof(AdFailedToLoadAction),
            typeof(Action<int>),
            typeof(AdMobNativeView));
        
        public Action AdOpenedAction
        {
            get => (Action)GetValue(AdOpenedActionProperty);
            set => SetValue(AdOpenedActionProperty, value);
        }

        public static readonly BindableProperty AdOpenedActionProperty = BindableProperty.Create(
            nameof(AdOpenedAction),
            typeof(Action),
            typeof(AdMobNativeView));
        
        public Action AdLeftApplicationAction
        {
            get => (Action)GetValue(AdLeftApplicationActionProperty);
            set => SetValue(AdLeftApplicationActionProperty, value);
        }

        public static readonly BindableProperty AdLeftApplicationActionProperty = BindableProperty.Create(
            nameof(AdLeftApplicationAction),
            typeof(Action),
            typeof(AdMobNativeView));
        
        public Action AdLoadedAction
        {
            get => (Action)GetValue(AdLoadedActionProperty);
            set => SetValue(AdLoadedActionProperty, value);
        }

        public static readonly BindableProperty AdLoadedActionProperty = BindableProperty.Create(
            nameof(AdLoadedAction),
            typeof(Action),
            typeof(AdMobNativeView));
        
        public Action AdRenderedAction
        {
            get => (Action)GetValue(AdRenderedActionProperty);
            set => SetValue(AdRenderedActionProperty, value);
        }

        public static readonly BindableProperty AdRenderedActionProperty = BindableProperty.Create(
            nameof(AdRenderedAction),
            typeof(Action),
            typeof(AdMobNativeView));
        
        public Action AdLoadingAction
        {
            get => (Action)GetValue(AdLoadingActionProperty);
            set => SetValue(AdLoadingActionProperty, value);
        }

        public static readonly BindableProperty AdLoadingActionProperty = BindableProperty.Create(
            nameof(AdLoadingAction),
            typeof(Action),
            typeof(AdMobNativeView));

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

        internal void TriggerAdClicked()
        {
            OnAdClicked?.Invoke(this, null);
            AdClickedAction?.Invoke();
        }

        internal void TriggerAdClosed()
        {
            OnAdClosed?.Invoke(this, null);
            AdClosedAction?.Invoke();
        }

        internal void TriggerAdImpression()
        {
            OnAdImpression?.Invoke(this, null);
            AdImpressionAction?.Invoke();
        }

        internal void TriggerAdFailedToLoad(int errorCode)
        {
            OnAdFailedToLoad?.Invoke(this, errorCode);
            AdFailedToLoadAction?.Invoke(errorCode);
        }

        internal void TriggerAdOpened()
        {
            OnAdOpened?.Invoke(this, null);
            AdOpenedAction?.Invoke();
        }

        internal void TriggerAdLeftApplication()
        {
            OnAdLeftApplication?.Invoke(this, null);
            AdLeftApplicationAction?.Invoke();
        }

        internal void TriggerAdLoaded()
        {
            OnAdLoaded?.Invoke(this, null);
            AdLoadedAction?.Invoke();
        }

        internal void TriggerAdRendered()
        {
            OnAdRendered?.Invoke(this, null);
            AdRenderedAction?.Invoke();
        }

        internal void TriggerAdLoading()
        {
            OnAdLoading?.Invoke(this, null);
            AdLoadingAction?.Invoke();
        }
    }
}
