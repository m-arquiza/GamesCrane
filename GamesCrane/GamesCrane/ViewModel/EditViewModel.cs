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
    /// <summary>
    /// The <c>EditViewModel</c> handles all navigation to different pages.
    /// </summary>
    public class EditViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand NavigateToAddCommand { get; }
        public ICommand ReturnToMainScreenCommand { get; }
        public ICommand StartSwitchCommand { get; }
        public ICommand StartRemoveCommand { get; }

        public EditViewModel()
        {
            Frame frame = App.RootFrame;
            _navigationService = new NavigationService(frame);

            NavigateToAddCommand = new RelayCommand(NavigateToAdd);
            ReturnToMainScreenCommand = new RelayCommand(GoBack);

            StartSwitchCommand = new RelayCommand(NavigateToMainSwitchState);
            StartRemoveCommand = new RelayCommand(NavigateToMainRemoveState);
        }

        /// <summary>
        /// Navigates to the Edit page.
        /// </summary>
        private void NavigateToAdd()
        {
            _navigationService.Navigate(typeof(AddPage));
        }
        /// <summary>
        /// Navigates back to the main page.
        /// </summary>
        private void GoBack()
        {
            _navigationService.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Navigates back to the main page but sets app state to switch.
        /// </summary>
        private void NavigateToMainSwitchState()
        {
            _navigationService.Navigate(typeof(MainPage), "switch");
        }

        /// <summary>
        /// Navigates back to the main page but sets app state to remove.
        /// </summary>
        private void NavigateToMainRemoveState()
        {
            _navigationService.Navigate(typeof(MainPage), "remove");
        }

    }
}
