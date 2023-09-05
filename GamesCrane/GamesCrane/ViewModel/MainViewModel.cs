using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using GamesCrane.Services;
using GamesCrane.View;
using Microsoft.UI.Xaml.Controls;

namespace GamesCrane.ViewModel
{
    public class MainViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand NavigateToPageCommand { get; }

        private Dictionary<string, string> _newGame;

        public MainViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);
            NavigateToPageCommand = new RelayCommand(NavigateToPage);
        }

        private void NavigateToPage()
        {
            _navigationService.Navigate(typeof(EditPage));
        }


        public Dictionary<string, string> NewGame
        {
            get { return _newGame; }
            set
            {
                if (_newGame != value)
                {
                    _newGame = value;
                    OnPropertyChanged(nameof(_newGame));
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
