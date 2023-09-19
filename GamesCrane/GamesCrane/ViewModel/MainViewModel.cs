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
    /// <summary>
    /// The <c>MainViewModel</c> represents the vending machine and all its information.
    /// </summary>
    public class MainViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand NavigateToEditCommand { get; }
        public ICommand SaveCommand { get; }

        private Game _newGame;
        private Game _vendedGame;
        private Game[,] _games;
        private int _gamesCount;

        private bool _dataLoaded;

        private string _state;
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
            
            _dataLoaded = false;

            _state = "default";

            OnLoad();
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

        public Game AddGame(Game gameToAdd)
        {
            Game currGame = new Game(gameToAdd);

            GamesCount++;
            currGame.NumIndex = _gamesCount;
            currGame.VendIndex = GameIndex(_gamesCount);

            int[] index = currGame.VendIndex;
            if (index[0] != -1)
            {
                _games[index[0], index[1]] = currGame;
                return currGame;
            }
            else
            {
                return null;
            }
        }

        public void DeleteGame(Game gameToDelete)
        {
            if(GamesCount != 0)
            {
                for (int row = gameToDelete.VendIndex[0]; row < 3; row++)
                {
                    for (int col = gameToDelete.VendIndex[1]; col < 5; col++)
                    {
                        if (gameToDelete.NumIndex == GamesCount)
                        {
                            Games[row, col] = null;
                            GamesCount--;
                            return;
                        }

                        if (col != 4)
                        {
                            Games[row, col] = Games[row, col + 1];
                            LowerGameIndex(Games[row, col]);
                        }
                        else
                        {
                            if (row != 2)
                            {
                                Games[row, col] = Games[row + 1, 0];
                                LowerGameIndex(Games[row, col]);
                            }
                            else
                            {
                                Games[row, col] = null;
                            }

                        }
                    }
                }
                GamesCount--;
            }
            OnPropertyChanged(nameof(Games));
        }

        /// <summary>
        /// Lowers the specified game's index by 1.
        /// <param name="game">game to lower index</param> 
        /// </summary>
        private void LowerGameIndex(Game game)
        {
            if (game != null)
            {
                game.NumIndex--;
            }
        }

        /// <summary>
        /// Swaps the given game and the currently vended game's locations.
        /// <param name="original">game the vended game will swap with</param> 
        /// </summary>
        public void SwapGame(Game original)
        {
            int[] swapVendIndex = VendedGame.VendIndex;
            int[] origVendIndex = original.VendIndex;
            VendedGame.VendIndex = origVendIndex;
            original.VendIndex = swapVendIndex;

            int swapNumIndex = VendedGame.NumIndex;
            int origNumIndex = original.NumIndex;
            VendedGame.NumIndex = origNumIndex;
            original.NumIndex = swapNumIndex;

            Games[swapVendIndex[0], swapVendIndex[1]] = original;
            Games[origVendIndex[0], origVendIndex[1]] = VendedGame;
        }

        public int GamesCount
        {
            get { return _gamesCount; }
            set
            {
                if (_gamesCount != value)
                {   
                    _gamesCount = value;
                    OnPropertyChanged(nameof(GamesCount));
                }
            }
        }

        public bool IsDataLoaded
        {
            get { return _dataLoaded; }
            set
            {
                if (_dataLoaded != value)
                {
                    _dataLoaded = value;
                    OnPropertyChanged(nameof(IsDataLoaded));
                }
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
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
                    OnPropertyChanged(nameof(Games));
                }
            }
        }

        /// <summary>
        /// Given a game's index, converts index to its index in the 2d array.
        /// <param name="gameNum">index to convert</param> 
        /// <returns>
        /// The 2d index where the first element is the outer array index 
        /// and the second element is the inner array index. </returns>
        /// </summary>
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

        /// <summary>
        /// Given clicked image, returns the associated game.
        /// <param name="imageName">image clicked</param> 
        /// <returns> Game associated with the clicked image. </returns>
        /// </summary>
        public Game GetGame(string imageName)
        {
            int gameNum = int.Parse(imageName.Where(Char.IsDigit).ToArray());
            return GetGame(gameNum);
        }

        /// <summary>
        /// The normal index, returns the associated game.
        /// <param name="regIndex">index to convert to 2d index</param> 
        /// <returns> Game associated with the clicked image. </returns>
        /// </summary>
        public Game GetGame(int regIndex)
        {
            int[] vendNum = GameIndex(regIndex);
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

        /// <summary>
        /// Navigates to the Edit page.
        /// </summary>
        private void NavigateToEdit()
        {
            _navigationService.Navigate(typeof(EditPage));
        }

        /// <summary>
        /// Saves games to file.
        /// </summary>
        private async void OnSave()
        {
            await AppStateManagerService.SaveAppStateAsync(Games);
        }

        /// <summary>
        /// Loads games from file.
        /// </summary>
        private async void OnLoad()
        {
            Object[] data = await AppStateManagerService.LoadAppStateAsync();
            Games = (Game[,])data[1];
            GamesCount = (int)data[0];
            IsDataLoaded = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
