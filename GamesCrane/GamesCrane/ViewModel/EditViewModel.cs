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
using GamesCrane.Services;
using Windows.Storage;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using GamesCrane.Model;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml;
using System.IO;


namespace GamesCrane.ViewModel
{
    public class EditViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand NavigateToAddCommand { get; }
        public ICommand ReturnToMainScreenCommand { get; }
        public EditViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);

            NavigateToAddCommand = new RelayCommand(NavigateToAdd);
            ReturnToMainScreenCommand = new RelayCommand(GoBack);
        }

        private void NavigateToAdd()
        {
            _navigationService.Navigate(typeof(AddPage));
        }
        private void GoBack()
        {
            _navigationService.Navigate(typeof(MainPage));
        }

    }
}
