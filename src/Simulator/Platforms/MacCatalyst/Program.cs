using UIKit;

namespace Simulator
{
    /// <summary>
    /// This is the main entry point of the application.
    /// </summary>
    public class Program
	{
        /// <summary>
        /// if you want to use a different Application Delegate
        /// class from "AppDelegate" you can specify it here.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) => UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
