using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using Microsoft.Maui.Storage;
using Simulator.ViewModel;

namespace Simulator;

/// <summary>
/// MainPage Class
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Constructor
    /// </summary>
	public MainPage()
	{
		InitializeComponent();
        WeakReferenceMessenger.Default.Register<OpenFileViewModel, OpenFileRequestMessage>(this, (r, m) => m.value(r.Username));
        WeakReferenceMessenger.Default.Register<SaveFileViewModel, SaveFileRequestMessage>(this, (r, m) => m.Reply(r.Username));
    }

    private void OpenFileMessageReceived(OpenFileViewModel model, OpenFileRequestMessage message)
    {
        var openFile = new OpenFile();
        openFile.ShowDialog();
    }

    private void SaveFileMessageReceived(SaveFileViewModel model, SaveFileRequestMessage message)
    {
        var saveFile = new SaveFile { DataContext = new SaveFileViewModel(message) };
        saveFile.ShowDialog();
    }
}
