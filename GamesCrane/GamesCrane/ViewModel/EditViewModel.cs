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

        private Game NewGame;
        public ICommand ReturnToMainScreenCommand { get; }
        public EditViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);

            ReturnToMainScreenCommand = new RelayCommand(GoBack);

            var appInstance = App.Current as App;
            m_window = appInstance.windowReference;


            NewGame = new Game();
            NewGame.Title = "Untitled Game";
            NewGame.ImagePath = "ms-appx:///Assets/StarsBorder.png";
        }

        public async void CheckDetails()
        {
            if (GameTitle.Equals("Untitled Game") |
                GameImagePath.Equals("ms-appx:///Assets/StarsBorder.png"))
            {
                Debug.WriteLine("title or path bad");
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
                Debug.WriteLine("title n path... good");
                SendDetails();
            }
        }

        public bool verifyPath()
        {
            //string programFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            //string filePath = Path.Combine(programFilesFolder, "Minecraft Launcher\\MinecraftLauncher.exe");
            string filePath = GamePath.Trim('"');
            if (File.Exists(filePath))
            {
                string fileExtension = Path.GetExtension(filePath);
                if (string.Equals(fileExtension, ".exe", StringComparison.OrdinalIgnoreCase))
                {
                    if (filePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                    {
                        Debug.WriteLine("sounds good to me");
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("invalid chars");
                    }
                }
                else
                {
                    Debug.WriteLine("doesn't end in exe");
                }
            }
            else
            {
                Debug.WriteLine("file no exist");
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
