#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace Simulator;

/// <summary>
/// Main Application
/// </summary>
public partial class App : Application
{
    const int WindowWidth = 1627;
    const int WindowHeight = 900;
    /// <summary>
    /// Constructor
    /// </summary>
	public App()
	{
		InitializeComponent();
/*
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(
            nameof(IWindow), (handler, view) =>
            {
#if MACCATALYST
                var size = new CoreGraphics.CGSize(WindowWidth, WindowHeight);
                handler.PlatformView.WindowScene.SizeRestrictions.MinimumSize = size;
                handler.PlatformView.WindowScene.SizeRestrictions.MaximumSize = size;
                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        handler.PlatformView.WindowScene.SizeRestrictions.MinimumSize = new CoreGraphics.CGSize(100, 100);
                        handler.PlatformView.WindowScene.SizeRestrictions.MaximumSize = new CoreGraphics.CGSize(5000, 5000);
                    });
                });
#endif
#if WINDOWS
                var mauiWindow = handler.VirtualView;
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
#endif
            });
*/
        MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        window.MinimumWidth = WindowWidth;
        window.MinimumHeight = WindowHeight;
        window.Width = WindowWidth;
        window.Height = WindowHeight;
        return window;
    }
}

