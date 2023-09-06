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

namespace GamesCrane.ViewModel
{
    public class EditViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand NavigateToPageCommand { get; }

        private string _gameTitle;
        private string _gamePath;
        private string _gameImagePath;

        public EditViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);
            NavigateToPageCommand = new RelayCommand(NavigateToPage);
        }

        private void NavigateToPage()
        {
            Dictionary<string, object> newGame = new Dictionary<string, object>();
            newGame.Add("title", _gameTitle);
            newGame.Add("path", _gamePath);
            newGame.Add("image", _gameImagePath);
            _navigationService.Navigate(typeof(MainPage), newGame);
        }


        public string GameTitle
        {
            get { return _gameTitle; }
            set
            {
                if (_gameTitle != value)
                {
                    _gameTitle = value;
                    OnPropertyChanged(nameof(GameTitle));
                }
            }
        }

        public string GamePath
        {
            get { return _gamePath; }
            set
            {
                if (_gamePath != value)
                {
                    _gamePath = value;
                    OnPropertyChanged(nameof(GamePath));
                }
            }
        }

        public string GameImagePath
        {
            get { return _gameImagePath; }
            set
            {
                if (_gameImagePath != value)
                {
                    _gameImagePath = value;
                    OnPropertyChanged(nameof(_gameImagePath));
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
