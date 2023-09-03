using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using GamesCrane.Model;
using Microsoft.UI.Xaml;
using Windows.Gaming.Preview.GamesEnumeration;
using Windows.UI.Xaml;


namespace GamesCrane.ViewModel
{
    /*  Contains controlls for interacting with the Main Window.
    */
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Visibility _addGameVis;
        private bool gamevis;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(){
            _addGameVis = Visibility.Collapsed;
            gamevis = false;
            AddGameCommand = new RelayCommand(launchAddGameControl);
        }

        // Updates and stores control visibility
        public Visibility AddGameVis
        {
            get { return _addGameVis; }
            set
            {
                if (_addGameVis != value)
                {
                    _addGameVis = value;
                    OnPropertyChanged();
                }
            }
        }

        // Affects AddGame control on user click
        public ICommand AddGameCommand { protected set; get; }

        // Displays or hides AddGame control 
        private void launchAddGameControl()
        {
            if(gamevis)
            {
                AddGameVis = Visibility.Collapsed;
            }
            else
            {
                AddGameVis = Visibility.Visible;
            }
            gamevis = !gamevis;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
