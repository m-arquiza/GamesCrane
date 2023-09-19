using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace GamesCrane.Services
{
    /// <summary>
    /// The <c>NavigationService</c> is used to navigate between pages.
    /// </summary>
    public class NavigationService
    {
        private Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        /// <summary>
        /// Navigaes to the given page.
        /// <param name="pageType">type of page to navigate to</param> 
        /// <param name="parameter">optional parameter to pass to the page</param> 
        /// </summary>
        public void Navigate(Type pageType, object parameter = null)
        {
            _frame.Navigate(pageType, parameter,
                            new DrillInNavigationTransitionInfo());
        }

    }
}
