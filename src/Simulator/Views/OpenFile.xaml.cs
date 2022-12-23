using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using NLog;
using Simulator.Messages;

namespace Simulator.Views;

/// <summary>
/// Interaction logic for OpenState.xaml
/// </summary>
public partial class OpenFile : ContentPage
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    /// <summary>
    /// Constructor
    /// </summary>
    public OpenFile()
    {
        InitializeComponent();
#if DEBUG
        _logger.Debug("Registering Messengers at: {time}, ", DateTimeOffset.UtcNow);
#endif
        WeakReferenceMessenger.Default.Register<OpenFileMessage>(this, (r, m) => NotificationMessageReceived(m));
    }

    private void NotificationMessageReceived(OpenFileMessage notificationMessage)
    {
#if DEBUG
        _logger.Debug("Got OpenFileMessage, notfication, at: {time}, ", DateTimeOffset.UtcNow);
#endif
        if (notificationMessage.Notification == "CloseFileWindow")
        {
            // We should close view
            //Close();
        }
    }
}
