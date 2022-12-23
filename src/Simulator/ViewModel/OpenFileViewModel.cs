using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Maui.Storage;
using Simulator.Model;
using Simulator.Messages;
using ProtoBuf;
using NLog;

namespace Simulator.ViewModel
{
    /// <summary>
    /// The ViewModel Used by the OpenFileView
    /// </summary>
    public class OpenFileViewModel : ObservableObject
    {
        #region Fields
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        #endregion

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
#if DEBUG
            _logger.Debug("OpenFileViewModel Constructor, at: {time}, ", DateTimeOffset.UtcNow);
#endif
            LoadProgramCommand = new RelayCommand(Load);
            CloseCommand = new RelayCommand(Close);
            SelectFileCommand = new RelayCommand(Select);

            InitalProgramCounter = "0x0000";
            MemoryOffset = "0x0000";
        }
        #endregion

        #region Private Methods
        private async void Load()
        {
#if DEBUG
            _logger.Debug("Load File, at: {time}, ", DateTimeOffset.UtcNow);
#endif
            var extension = Path.GetExtension(Filename);
            if (extension != null && extension.ToUpper() == ".BIN" && !await TryLoadBinFile())
                return;

            if (extension != null && extension.ToUpper() == ".6502" && !await TryLoad6502File())
                return;

            Close();
        }

        private async Task<bool> TryLoad6502File()
        {
            try
            {
                //var formatter = new BinaryFormatter();
                Stream stream = new FileStream(Filename, FileMode.Open);

                //var fileModel = (StateFileModel)formatter.Deserialize(stream);
                var fileModel = Serializer.Deserialize<StateFileModel>(stream);
                fileModel.FilePath = Filename;
                stream.Close();

                WeakReferenceMessenger.Default.Send(new StateFileMessage(fileModel, "FileLoaded"));

                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to load file", "OK");
                _logger.Error("Unable to load file: \"{message}\", at: {time}, ", ex.Message, DateTimeOffset.UtcNow);
                return false;
            }
        }

        private async Task<bool> TryLoadBinFile()
        {
            int programCounter;
            try
            {
                programCounter = Convert.ToInt32(InitalProgramCounter, 16);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid ProgramCounter", "Unable to Parse ProgramCounter into int", "OK");
                _logger.Error("Unable to Parse ProgramCounter into int, at: {time}, ", DateTimeOffset.UtcNow);
                return false;
            }

            int memoryOffset;
            try
            {
                memoryOffset = Convert.ToInt32(MemoryOffset, 16);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Memory Offset", "Unable to Parse Memory Offset into int", "OK");
                _logger.Error("Unable to Parse Memory Offset into int, at: {time}, ", DateTimeOffset.UtcNow);
                return false;
            }

            byte[] program;
            try
            {
                program = File.ReadAllBytes(Filename);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Program Binary", "Unable to Open Program Binary", "OK");
                _logger.Error("Unable to Open Program Binary, at: {time}, ", DateTimeOffset.UtcNow);
                return false;
            }

            string listing;
            try
            {
                listing = File.ReadAllText(string.Format("{0}.lst", Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename))));
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Program Listing", "Unable to Open Program Listing", "OK");
                _logger.Error("Unable to Open Program Listing, at: {time}, ", DateTimeOffset.UtcNow);
                return false;
            }

            WeakReferenceMessenger.Default.Send(new AssemblyFileMessage(new AssemblyFileModel
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
            WeakReferenceMessenger.Default.Send(new CloseFileMessage("CloseFileWindow"));
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
                _logger.Error("Caught Error: \"{message}\", at: {time}, ", ex.Message, DateTimeOffset.UtcNow);
            }
        }
        #endregion
    }
}
