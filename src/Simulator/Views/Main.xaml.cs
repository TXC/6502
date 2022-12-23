using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Maui.Storage;
using Simulator.ViewModel;
using Simulator.Messages;
using Microsoft.Maui.Controls;
using NLog;

namespace Simulator.Views;

/// <summary>
/// MainPage Class
/// </summary>
public partial class Main : ContentPage
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Constructor
    /// </summary>
	public Main()
    {
#if DEBUG
        _logger.Debug("Registering Messengers at: {time}, ", DateTimeOffset.UtcNow);
#endif
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<OpenFileMessage>(this, (r, m) => NotificationMessageReceived(m));
        WeakReferenceMessenger.Default.Register<StateFileMessage>(this, (r, m) => NotificationMessageReceived(m));
    }

    private void NotificationMessageReceived(OpenFileMessage notification)
    {
#if DEBUG
        _logger.Debug("Got OpenFileMessage, notfication, at: {time}, ", DateTimeOffset.UtcNow);
#endif
        if (notification.Notification == "OpenFileWindow")
        {
            //var openFile = new OpenFile();
            //openFile.ShowDialog();
        }
    }

    private void NotificationMessageReceived(StateFileMessage notification)
    {
#if DEBUG
        _logger.Debug("Got StateFileMessage, notfication, at: {time}, ", DateTimeOffset.UtcNow);
#endif
        if (notification.Notification == "SaveFileWindow")
        {
            //var saveFile = new SaveFile { DataContext = new SaveFileViewModel(notification.Content) };
            //saveFile.ShowDialog();
        }
    }
}
