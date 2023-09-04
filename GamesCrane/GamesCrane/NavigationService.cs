using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace GamesCrane
{
    public class NavigationService
    {
        private Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void Navigate(Type pageType, object parameter = null)
        {
            _frame.Navigate(pageType, parameter);
        }

    }
}
