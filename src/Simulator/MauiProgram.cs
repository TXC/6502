using Microsoft.Extensions.Logging;
//using Simulator.ViewModel;

namespace Simulator;

/// <summary>
/// MAUI Program
/// </summary>
public static class MauiProgram
{
	/// <summary>
	/// MAUI App
	/// </summary>
	/// <returns></returns>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<ViewModel.MainViewModel>();
        builder.Services.AddTransient<ViewModel.OpenFileViewModel>();
        builder.Services.AddTransient<ViewModel.SaveFileViewModel>();
        //builder.Services.AddTransient<Views.MainPage>();
        //builder.Services.AddTransient<App>();

        //builder.Services.AddSingleton<ViewModel.MainViewModel>();
        //builder.Services.AddSingleton<ViewModel.OpenFileViewModel>();
        //builder.Services.AddSingleton<ViewModel.SaveFileViewModel>();
        //builder.Services.AddSingleton<Views.MainPage>();
        //builder.Services.AddSingleton<App>();

        return builder.Build();
	}
}

