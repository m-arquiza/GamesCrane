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

namespace GamesCrane.ViewModel
{
    public class EditViewModel
    {
        private readonly NavigationService _navigationService;

        private Game NewGame;
        public ICommand ReturnToMainScreen { get; }

        public EditViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);

            ReturnToMainScreen = new RelayCommand(GoBack);

            NewGame = new Game();
            NewGame.Title = "Untitled Game";
            NewGame.ImagePath = "ms-appx:///Assets/StarsBorder.png";
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
