using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
using NLog;
using Simulator.Messages;

namespace Simulator.Views;

/// <summary>
/// Interaction logic for SaveState.xaml
/// </summary>
public partial class SaveFile : ContentPage
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    /// <summary>
    /// Constructor
    /// </summary>
    public SaveFile()
    {
        //InitializeComponent();
#if DEBUG
        _logger.Debug("Registering Messengers at: {time}, ", DateTimeOffset.UtcNow);
#endif
        WeakReferenceMessenger.Default.Register<SaveFileMessage>(this, (r, m) => NotificationMessageReceived(m));
    }

    private void NotificationMessageReceived(SaveFileMessage notificationMessage)
    {
#if DEBUG
        _logger.Debug("Got SaveFileMessage, notfication, at: {time}, ", DateTimeOffset.UtcNow);
#endif
        if (notificationMessage.Notification == "CloseSaveFileWindow")
        {
            // We should close view
            //Close();
        }
    }
}
