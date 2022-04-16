using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {
        public ShopWindow()
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new Pages.AuthorizationPage());
        }

        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            if (frame.NavigationService.CanGoForward)
                frame.NavigationService.GoForward();
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {

            if (frame.NavigationService.CanGoBack)
                frame.NavigationService.GoBack();
        }
    }
}
