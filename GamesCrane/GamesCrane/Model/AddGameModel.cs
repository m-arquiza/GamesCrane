using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesCrane.Model
{
    public class AddGameModel : INotifyPropertyChanged
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged(nameof(Path));
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
