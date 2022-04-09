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

        private Dictionary<string, Func<Product, object>> Sortings;

        private int startIndex;
        private int separator;

        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts();
            showedProducts = Products.ToList();
            Units = DataAccess.GetUnits();
            Units.Add(new Unit { Name = "Все" });

            Sortings = new Dictionary<string, Func<Product, object>>
            {
                { "Сначала старые", x => x.AddDate },//reverse
                { "Сначала новые", x => x.AddDate },
                { "А-Я", x => x.Name },
                { "Я-А", x => x.Name } //reverse
            };

            startIndex = 0;
            separator = Convert.ToInt32((cmBox.SelectedItem as ComboBoxItem).Content.ToString());
            cbMonthFilter.SelectedIndex = 0;
            cmBoxUnits.SelectedIndex = 3;
            this.DataContext = this;
            GoPagination();
        }

        private void GoPagination()
        {
            if (startIndex < showedProducts.Count)
            {
                int test = (showedProducts.Count / (separator + startIndex)) > 0 ? separator : showedProducts.Count % separator;
                var search = showedProducts.GetRange(startIndex, test);
                gridProducts.ItemsSource = search;
                lblPages.Content = $"{startIndex + test} из {showedProducts.Count}";
            }
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

        private void ApplyFilters()
        {
            if (cbMonthFilter.SelectedItem != null & cmBoxUnits.SelectedItem != null)
            {
                var text = tbSearch.Text;
                var selectedFilter = (cbMonthFilter.SelectedItem as ComboBoxItem).Content.ToString();

                var unit = cmBoxUnits.SelectedItem as Unit;

                if (unit.Name == "Все")
                    showedProducts = Products.ToList();
                else
                    showedProducts = Products.Where(p => p.UnitId == unit.Id).ToList();

                if (selectedFilter == "Все")
                {
                    gridProducts.ItemsSource = showedProducts;
                }
                else
                {
                    showedProducts = showedProducts.Where(p => p.AddDate.Month == DateTime.Now.Month).ToList();
                }

                showedProducts = showedProducts.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();
                gridProducts.ItemsSource = showedProducts;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
            GoPagination();
        }

        private void cmBoxUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
            GoPagination();
        }

        private void cbMonthFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
            GoPagination();
        }

        private void gridProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = gridProducts.SelectedItems.Count != 0;
            btnEdit.IsEnabled = gridProducts.SelectedItems.Count != 0;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ProductPage(gridProducts.SelectedItem as Product));
        }
    }
}
