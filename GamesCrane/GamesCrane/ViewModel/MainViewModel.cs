using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        private Dictionary<string, object> _newGame;
        private Dictionary<string, object>[,] _games;
        private int _gamesCount;
        public MainViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);
            NavigateToPageCommand = new RelayCommand(NavigateToPage);

            _newGame = new Dictionary<string, object>();
            _games = new Dictionary<string, object>[3, 5];
            _gamesCount = 0;
        }

        public Dictionary<string, object> NewGame
        {
            get { return _newGame; }
            set
            {
                if (_newGame != value)
                {
                    GamesCount++;
                    value.Add("numIndex", _gamesCount);
                    value.Add("vendIndex", GameIndex(_gamesCount));

                    _newGame = value;

                    OnPropertyChanged(nameof(_newGame));

                    GameAdded(_newGame);
                }
            }
        }

        private void GameAdded(Dictionary<string, object> gameToAdd)
        {
            Dictionary<string, object> currGame = new Dictionary<string, object>(gameToAdd);
            int[] index = (int[]) currGame["vendIndex"];
            if (index[0] != -1)
            {
                _games[index[0], index[1]] = currGame;
            }
            else
            {
                Console.WriteLine("Too many games!");
                _gamesCount--;
            }
        }

        public int GamesCount
        {
            get { return _gamesCount; }
            set
            {
                if (_gamesCount != value)
                {   
                    _gamesCount = value;
                    OnPropertyChanged(nameof(_gamesCount));
                }
            }
        }

        public Dictionary<string, object>[,] Games
        {
            get { return _games; }
            set
            {
                if (_games != value)
                {
                    _games = value;
                    OnPropertyChanged(nameof(_games));
                }
            }
        }

        public int[] GameIndex(int gameNum)
        {
            int[] index = new int[2];
            if (gameNum >= 1 && gameNum <= 15)
            {
                index[0] = (gameNum - 1) / 5;
                index[1] = (gameNum - 1) % 5;
            }
            else
            {
                index[0] = -1; ;
                index[1] = -1;
            }
            return index;
        }


        public Dictionary<string, object> HandleGameTap(string imageName)
        {
            int gameNum = int.Parse(imageName.Where(Char.IsDigit).ToArray());
            int[] vendNum = GameIndex(gameNum);
            Dictionary<string, object> game = Games[vendNum[0], vendNum[1]];

            return game;
        }

        public ICommand NavigateToPageCommand { get; }
        private void NavigateToPage()
        {
            _navigationService.Navigate(typeof(EditPage));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
