using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Core;
namespace Shop.Pages
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Unit> Units { get; set; }
        public List<Product> showedProducts { get; set; }
        private int startIndex;
        private int separator;

        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts();
            Units = DataAccess.GetUnits();
            startIndex = 0;
            separator = Convert.ToInt32((cmBox.SelectedItem as ComboBoxItem).Content.ToString());
            GoPagination();
        }

        private void GoPagination()
        {
            int test = (Products.Count / (separator + startIndex)) > 0 ? separator : Products.Count % separator;
            showedProducts = Products.ToList().GetRange(startIndex, test);
            gridProducts.ItemsSource = showedProducts;
            this.DataContext = this;
            lblPages.Content = $"{startIndex + test} из {Products.Count}";
        }

        private void btnGoPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (startIndex != 0)
                startIndex -= Convert.ToInt32((cmBox.SelectedItem as ComboBoxItem).Content.ToString());
            GoPagination();
        }

        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            if(startIndex + Convert.ToInt32((cmBox.SelectedItem as ComboBoxItem).Content.ToString()) < Products.Count)
                startIndex += Convert.ToInt32((cmBox.SelectedItem as ComboBoxItem).Content.ToString());
            GoPagination();
        }

        private void cmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmBox.Text != "")
            {
                separator = Convert.ToInt32((cmBox.SelectedItem as ComboBoxItem).Content.ToString());
                startIndex = 0;
                GoPagination();
            }
        }
    }
}
