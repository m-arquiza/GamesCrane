using GamesCrane.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using GamesCrane.Services;
using Windows.Storage;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using GamesCrane.Model;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml;
using System.IO;


namespace GamesCrane.ViewModel
{
    public class EditViewModel
    {
        private Window m_window;
        private readonly NavigationService _navigationService;

        private string _filePathError;
        private Game NewGame;
        public ICommand ReturnToMainScreenCommand { get; }
        public EditViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);

            ReturnToMainScreenCommand = new RelayCommand(GoBack);

            var appInstance = App.Current as App;
            m_window = appInstance.windowReference;

            _filePathError = "Unknown";
            NewGame = new Game();
            NewGame.Title = "Untitled Game";
            NewGame.ImagePath = "ms-appx:///Assets/StarsBorder.png";
        }

        public async void CheckDetails()
        {
            if (GameTitle.Equals("Untitled Game") |
                GameImagePath.Equals("ms-appx:///Assets/StarsBorder.png"))
            {
                AddGameDetailsContentDialog dialog = new AddGameDetailsContentDialog();
                if (m_window.Content is FrameworkElement fe)
                {
                    dialog.XamlRoot = fe.XamlRoot;
                }
                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    SendDetails();
                }
            }
            else
            {
                SendDetails();
            }
        }

        public bool verifyPath()
        {
            //string programFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            //string filePath = Path.Combine(programFilesFolder, "Minecraft Launcher\\MinecraftLauncher.exe");
            string filePath = GamePath.Replace("\"", "");

            if (GamepathHasFlags)
            {

                int exeIndex = filePath.IndexOf(".exe");
                if (exeIndex >= 0)
                {
                    GamePath = filePath.Substring(0, exeIndex + 4);
                    NewGame.PathFlags = filePath.Substring(exeIndex + 4).Trim();
                    filePath = GamePath;
                Debug.WriteLine("path: " + filePath);
                Debug.WriteLine("flags:" + NewGame.PathFlags);
                }
                else
                {
                    FilePathErrorDisplayText = "File does not end in .exe! Are you missing the executable in the path?";
                    return false;
                }
            }

            if (File.Exists(filePath))
            {
                string fileExtension = Path.GetExtension(filePath);
                if (string.Equals(fileExtension, ".exe", StringComparison.OrdinalIgnoreCase))
                {
                    if (filePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                    {
                        return true;
                    }
                    else
                    {
                        FilePathErrorDisplayText = "Path includes invalid characters!";
                    }
                }
                else
                {
                    FilePathErrorDisplayText = "File does not end in .exe! Are you missing the executable in the path?";
                }
            }
            else
            {
                FilePathErrorDisplayText = "File does not exist! System cannot find the application based on the information you've given. Please make sure that you've selected the right option based on the given path (does this path include flags (marked as .exe -<somethinghere>) or do you need to run this program as administrator?";
            }
            return false;
        }

        public void SendDetails()
        {
            Game toAdd = new Game(NewGame);
            _navigationService.Navigate(typeof(MainPage), NewGame);
        }

        public void GoBack()
        {
            _navigationService.Navigate(typeof(MainPage));
        }

        public string FilePathErrorDisplayText
        {
            get { return _filePathError; }
            set
            {
                if (_filePathError != value)
                {
                    _filePathError = value;
                    OnPropertyChanged(nameof(FilePathErrorDisplayText));
                }
            }
        }
        public string GameTitle
        {
            get { return NewGame.Title; }
            set
            {
                if (NewGame.Title != value)
                {
                    NewGame.Title = value;
                    OnPropertyChanged(nameof(GameTitle));
                }
            }
        }

        public string GamePath
        {
            get { return NewGame.Path; }
            set
            {
                if (NewGame.Path != value)
                {
                    NewGame.Path = value;
                    OnPropertyChanged(nameof(GamePath));
                }
            }
        }

        public string GameImagePath
        {
            get { return NewGame.ImagePath; }
            set
            {
                if (NewGame.ImagePath != value)
                {
                    NewGame.ImagePath = value;
                    OnPropertyChanged(nameof(GameImagePath));
                }
            }
        }

        public Boolean GameNeedsAdmin
        {
            get { return NewGame.NeedsAdmin; }
            set
            {
                if (NewGame.NeedsAdmin != value)
                {
                    NewGame.NeedsAdmin = value;
                    OnPropertyChanged(nameof(GameNeedsAdmin));
                }
            }
        }

        public Boolean GamepathHasFlags
        {
            get { return NewGame.HasFlags; }
            set
            {
                if (NewGame.HasFlags != value)
                {
                    NewGame.HasFlags = value;
                    OnPropertyChanged(nameof(GamepathHasFlags));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
