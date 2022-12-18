using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Maui.Storage;
using Simulator.Model;

namespace Simulator.ViewModel
{
    /// <summary>
    /// The ViewModel Used by the OpenFileView
    /// </summary>
    public class OpenFileViewModel : ObservableObject
    {
        #region Properties
        /// <summary>
        /// The Relay Command used to Load a Program
        /// </summary>
        public RelayCommand LoadProgramCommand { get; set; }

        /// <summary>
        /// The Relay Command used to close the dialog
        /// </summary>
        public RelayCommand CloseCommand { get; set; }

        /// <summary>
        /// The Relay Command used to select a file
        /// </summary>
        public RelayCommand SelectFileCommand { get; set; }

        /// <summary>
        /// The Name of the file being opened
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The Initial Program Counter, used only when opening a Binary File. Not used when opening saved state.
        /// </summary>
        public string InitalProgramCounter { get; set; }

        /// <summary>
        /// The inital memory offset. Determines where in memory the program begins loading to.
        /// </summary>
        public string MemoryOffset { get; set; }

        /// <summary>
        /// Tells the UI if the file has been selected succesfully
        /// </summary>
        public bool LoadEnabled { get { return !string.IsNullOrEmpty(Filename); } }

        /// <summary>
        /// Tells the UI if the file type is not a state file. This Property prevents the InitialProgram Counter and Memory Offset from being enabled.
        /// </summary>
        public bool IsNotStateFile
        {
            get
            {
                if (string.IsNullOrEmpty(Filename))
                    return true;

                return !Filename.EndsWith(".6502");
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a new instance of the OpenFileViewModel
        /// </summary>
        public OpenFileViewModel()
        {
            LoadProgramCommand = new RelayCommand(Load);
            CloseCommand = new RelayCommand(Close);
            SelectFileCommand = new RelayCommand(Select);

            InitalProgramCounter = "0x0000";
            MemoryOffset = "0x0000";
        }
        #endregion

        #region Private Methods
        private void Load()
        {
            var extension = Path.GetExtension(Filename);
            if (extension != null && extension.ToUpper() == ".BIN" && !TryLoadBinFile())
                return;

            if (extension != null && extension.ToUpper() == ".6502" && !TryLoad6502File())
                return;

            Close();
        }

        private bool TryLoad6502File()
        {
            var formatter = new BinaryFormatter();
            Stream stream = new FileStream(Filename, FileMode.Open);

            var fileModel = (StateFileModel)formatter.Deserialize(stream);
            fileModel.FilePath = Filename;

            stream.Close();

            WeakReferenceMessenger.Default.Send(new NotificationMessage<StateFileModel>(fileModel, "FileLoaded"));

            return true;
        }

        private bool TryLoadBinFile()
        {
            int programCounter;
            try
            {
                programCounter = Convert.ToInt32(InitalProgramCounter, 16);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to Parse ProgramCounter into int");
                return false;
            }

            int memoryOffset;
            try
            {
                memoryOffset = Convert.ToInt32(MemoryOffset, 16);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to Parse Memory Offset into int");
                return false;
            }

            byte[] program;
            try
            {
                program = File.ReadAllBytes(Filename);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to Open Program Binary");
                return false;
            }

            string listing;
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                listing = File.ReadAllText(string.Format("{0}.lst", Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename))));
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to Open Program Listing");
                return false;
            }

            WeakReferenceMessenger.Default.Send(new NotificationMessage<AssemblyFileModel>(new AssemblyFileModel
            {
                InitialProgramCounter = programCounter,
                MemoryOffset = memoryOffset,
                Listing = listing,
                Program = program,
                FilePath = Filename
            }, "FileLoaded"));

            return true;
        }

        private static void Close()
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage("CloseFileWindow"));
        }

        private async void Select()
        {
            // { DefaultExt = ".bin", Filter = "All Files (*.bin, *.6502)|*.bin;*.6502|Binary Assembly (*.bin)|*.bin|6502 Simulator Save State (*.6502)|*.6502" }

            var fileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "bin", "6502" } }, // UTType values
                    { DevicePlatform.Android, new[] { "application/octet-stream" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".bin", ".6502" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { "bin", "6502" } }, // UTType values
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a file",
                FileTypes = fileTypes,
            };
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("bin", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("6502", StringComparison.OrdinalIgnoreCase))
                    {
                        Filename = result.FileName;
                        OnPropertyChanged(nameof(Filename));
                        OnPropertyChanged(nameof(LoadEnabled));
                        OnPropertyChanged(nameof(IsNotStateFile));

                        //using var stream = await result.OpenReadAsync();
                        //var image = ImageSource.FromStream(() => stream);
                    }
                }
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
        }
        #endregion
    }

    // A sample message with a username value
    public sealed class OpenFileChangedMessage : ValueChangedMessage<string>
    {
        public OpenFileChangedMessage(string value) : base(value)
        {
        }
    }

    // A sample request message to get the current username
    public sealed class OpenFileRequestMessage : RequestMessage<string>
    {
    }
}
