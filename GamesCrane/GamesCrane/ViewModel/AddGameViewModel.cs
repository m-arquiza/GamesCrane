using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GamesCrane.Model;

namespace GamesCrane.ViewModel
{
    public class AddGameViewModel : INotifyPropertyChanged
    {
        private AddGameModel _newGame;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public AddGameViewModel()
        {
            NewGame = new AddGameModel();
        }

        public AddGameModel NewGame
        {
            get
            {
                Debug.WriteLine("inside get");
                return _newGame;
            }
            set
            {
                Debug.WriteLine("inside set");

                if (_newGame != value)
                {
                    _newGame = value;
                    OnPropertyChanged();
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
