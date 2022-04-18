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
using Core;

namespace Shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductIntakesPage.xaml
    /// </summary>
    public partial class ProductIntakesPage : Page
    {
        public List<ProductIntake> Intakes { get; set; }
        public List<Product> Products { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<ProductIntakeProduct> IntakeProducts { get; set; }


        public ProductIntake Intake { get; set; }

        public ProductIntakesPage()
        {
            InitializeComponent();

            Intake = new ProductIntake();
            Products = DataAccess.GetProducts().ToList();
            IntakeProducts = new List<ProductIntakeProduct>();
            dpDate.SelectedDate = DateTime.Now;

            gridProducts.SelectionMode = DataGridSelectionMode.Extended;

            Suppliers = DataAccess.GetSuppliers().ToList();
            cbSupplier.SelectedIndex = 0;
            DataContext = this;
        }

        public ProductIntakesPage(ProductIntake intake)
        {

            InitializeComponent();
            Intake = intake;
            Products = DataAccess.GetProducts().ToList();
            IntakeProducts = intake.ProductIntakeProduct.ToList();
            dpDate.SelectedDate = Intake.Data;
            gridProducts.SelectionMode = DataGridSelectionMode.Extended;
            grid.IsEnabled = Intake.StatusIntakeId != 1;

            Suppliers = DataAccess.GetSuppliers().ToList();
            cbSupplier.SelectedIndex = 0;
            tbSum.Text = ((int)Intake.TotalAmount).ToString();
            this.DataContext = this;
        }

        private void gridProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = (sender as Product);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var product = cbProduct.SelectedItem as Product;
            IntakeProducts.Add(new ProductIntakeProduct() { ProductId = product.Id, Product = product });

            Products.Remove(product);

            gridProducts.Items.Refresh();
        }


        private void gridProducts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.gridProducts.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= gridProducts_RowEditEnding;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();

                decimal sum = 0;
                foreach (ProductIntakeProduct product in gridProducts.ItemsSource)
                {
                    sum += product.Sum;
                }
                tbSum.Text = sum.ToString();
                (sender as DataGrid).RowEditEnding += gridProducts_RowEditEnding;
            }
            return;
        }
        private void btnConduct_Click(object sender, RoutedEventArgs e)
        {
            Intake.SupplierId = (cbSupplier.SelectedItem as Supplier).Id;
            Intake.TotalAmount = decimal.Parse(tbSum.Text);
            Intake.Data = (DateTime)dpDate.SelectedDate;
            Intake.StatusIntakeId = 2;
            Intake.IsDeleted = false;
            Intake.ProductIntakeProduct = IntakeProducts;
            Intake.StatusIntakeId = 1;

            DataAccess.SaveProductIntake(Intake);
            NavigationService.GoBack();
        }

    }
}
