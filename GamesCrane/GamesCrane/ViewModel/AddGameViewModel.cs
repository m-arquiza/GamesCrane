using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GamesCrane.Model;
using System.ComponentModel.Design;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GamesCrane.ViewModel
{
    public class AddGameViewModel : INotifyPropertyChanged
    {
        private AddGameModel _newGame;
        private String _pathToDisplay;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public AddGameViewModel()
        {
            NewGame = new AddGameModel();

            DisplayGameCommand = new RelayCommand(setDisplay);
        }

        public AddGameModel NewGame
        {
            get { return _newGame; }
            set
            {
                if (_newGame != value)
                {
                    _newGame = value;
                    OnPropertyChanged();
                }
            }
        }

        public String PathToDisplay
        {
            get { return _pathToDisplay; }
            set
            {
                if (_pathToDisplay != value)
                {
                    _pathToDisplay = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand DisplayGameCommand { protected set; get; }
            
        private void setDisplay()
        {
            PathToDisplay = _newGame.Path;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
