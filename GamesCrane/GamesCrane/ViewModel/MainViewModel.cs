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
using GamesCrane.Model;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml;
using Windows.Storage;
using CommunityToolkit.WinUI.UI.Controls;

namespace GamesCrane.ViewModel
{
    public class MainViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand NavigateToEditCommand { get; }
        public ICommand SaveCommand { get; }

        private Game _newGame;
        private Game _vendedGame;
        private Game[,] _games;
        private int _gamesCount;
        public MainViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);

            NavigateToEditCommand = new RelayCommand(NavigateToEdit);
            SaveCommand = new RelayCommand(OnSave);
            _newGame = new Game();
            _vendedGame = new Game();
            _games = new Game[3, 5];
            _gamesCount = 0;
        }

        public Game NewGame
        {
            get { return _newGame; }
            set
            {
                if (_newGame != value)
                {
                    GamesCount++;
                    value.NumIndex = _gamesCount;
                    value.VendIndex = GameIndex(_gamesCount);

                    _newGame = value;

                    OnPropertyChanged(nameof(_newGame));

                    GameAdded(_newGame);
                }
            }
        }
        public Game VendedGame
        {
            get { return _vendedGame; }
            set
            {
                if (_vendedGame != value)
                {
                    _vendedGame = value;

                    OnPropertyChanged(nameof(VendedGame));
                }
            }
        }

        private void GameAdded(Game gameToAdd)
        {
            Game currGame = new Game(gameToAdd);
            int[] index = currGame.VendIndex;
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

        public Game[,] Games
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


        public Game GetGame(string imageName)
        {
            int gameNum = int.Parse(imageName.Where(Char.IsDigit).ToArray());
            int[] vendNum = GameIndex(gameNum);
            Game game = Games[vendNum[0], vendNum[1]];

            return game;
        }

        public bool GameLaunch()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(VendedGame.Path);
            if (VendedGame.HasFlags)
            {
                startInfo.Arguments = VendedGame.PathFlags;
            }
            if (VendedGame.NeedsAdmin)
            {
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
            }

            try
            {
                System.Diagnostics.Process.Start(startInfo);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
                return false;
            }
        }

        private void NavigateToEdit()
        {
            _navigationService.Navigate(typeof(EditPage));
        }

        private async void OnSave()
        {
            Debug.WriteLine("saving");
            await AppStateManagerService.SaveAppStateAsync(Games);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
