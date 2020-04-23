using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dota2Handbook.Utilities
{
    public static class WebViewExtensions
    {
        public static string GetUriSource(WebView view) => 
            (string) view.GetValue(UriSourceProperty);

        public static void SetUriSource(WebView view, string value) => 
            view.SetValue(UriSourceProperty, value);

        public static readonly DependencyProperty UriSourceProperty = DependencyProperty.RegisterAttached("UriSource", typeof(string), typeof(WebViewExtensions), new PropertyMetadata(null, OnUriSourcePropertyChanged));

        private static void OnUriSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var webView = sender as WebView;

            if (e.NewValue != null && 
                !string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                var uri = new Uri(e.NewValue.ToString());
                webView.Navigate(uri);
            }
        }
    }
}