using GamesCrane.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace GamesCrane.ViewModel
{
    public class EditViewModel
    {
        private readonly NavigationService _navigationService;

        public ICommand NavigateToPageCommand { get; }

        public EditViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);
            NavigateToPageCommand = new RelayCommand(NavigateToPage);
        }

        private void NavigateToPage()
        {
            // Pass the data (e.g., Name) as a parameter when navigating
            _navigationService.Navigate(typeof(MainPage), DataToPass);
        }

        private string _dataToPass;

        public string DataToPass
        {
            get { return _dataToPass; }
            set
            {
                if (_dataToPass != value)
                {
                    _dataToPass = value;
                    OnPropertyChanged(nameof(DataToPass));
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
