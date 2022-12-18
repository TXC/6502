namespace Simulator;

/// <summary>
/// Main Application
/// </summary>

public partial class App : Application
{
    /// <summary>
    /// Constructor
    /// </summary>
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}

