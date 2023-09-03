using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace GamesCrane.ViewModel
{
    public class AddGameViewModel : INotifyPropertyChanged
    {
        private string _gamePath;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string GamePath
        {
            get
            {
                return _gamePath;
            }
            set
            {
                if (_gamePath != value)
                {
                    _gamePath = value;
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
