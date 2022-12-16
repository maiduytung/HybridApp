﻿using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Platform;
#if ANDROID
using AndroidX.Activity;
using MauiBlazorApp.Platforms.Android;
#endif

namespace MauiBlazorApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;
            blazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;
        }


        private void BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
#if ANDROID
            if (e.WebView.Context?.GetActivity() is not ComponentActivity activity)
            {
                throw new InvalidOperationException($"The permission-managing WebChromeClient requires that the current activity be a '{nameof(ComponentActivity)}'.");
            }

            e.WebView.Settings.JavaScriptEnabled = true;
            e.WebView.Settings.AllowFileAccess = true;
            e.WebView.Settings.MediaPlaybackRequiresUserGesture = false;
            e.WebView.Settings.SetGeolocationEnabled(true);
            e.WebView.Settings.SetGeolocationDatabasePath(e.WebView.Context?.FilesDir?.Path);
            e.WebView.SetWebChromeClient(new PermissionManagingBlazorWebChromeClient(e.WebView.WebChromeClient!, activity));
#elif IOS
            //e.Configuration.AllowsInlineMediaPlayback = true;
            //e.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;
#endif
        }

        private void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e)
        {
#if IOS || MACCATALYST                   
            e.Configuration.AllowsInlineMediaPlayback = true;
            e.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;
#endif
        }
    }
}
