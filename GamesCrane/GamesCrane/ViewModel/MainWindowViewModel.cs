using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GamesCrane.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private bool _clicked;
        //public bool Clicked
        //{
        //    get
        //    {
        //        return _clicked;
        //    }
        //    set
        //    {
        //        if (_clicked != value)
        //        {
        //            _clicked = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

    }
}
