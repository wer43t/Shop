﻿using System;
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
using Microsoft.Win32;
using System.IO;

namespace Shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public Product Product { get; set; }
        public List<Country> ProductCountries { get; set; }
        public List<Country> Countries { get; set; }
        public List<Unit> Units { get; set; }

        public ProductPage()
        {
            InitializeComponent();

            Product = new Product();
            Countries = DataAccess.GetCountries().ToList();
            Units = DataAccess.GetUnits().ToList();

            FillCountries();

            cbCounties.ItemsSource = Countries;
            lvCountries.ItemsSource = ProductCountries;

            cbUnits.ItemsSource = Units;

            spId.Visibility = Visibility.Hidden;
            this.DataContext = Product;
        }

        public ProductPage(Product product)
        {
            InitializeComponent();

            Product = product;
            Countries = DataAccess.GetCountries().ToList();
            Units = DataAccess.GetUnits().ToList();

            FillCountries();

            cbCounties.ItemsSource = Countries;
            lvCountries.ItemsSource = ProductCountries;

            cbUnits.ItemsSource = Units;
            cbUnits.SelectedItem = Product.Unit;

            this.DataContext = Product;
        }


        private void FillCountries()
        {
            ProductCountries = new List<Country>();
            var productCountries = Product.ProductCountry.Where(p => p.ProductId == Product.Id).ToList();
            foreach (var country in productCountries)
            {
                ProductCountries.Add(Countries.Where(c => c.Id == country.CountryId).FirstOrDefault());
            }
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            Product.Name = tbName.Text;
            Product.UnitId = (cbUnits.SelectedItem as Unit).Id;
            Product.Description = tbDescription.Text;
            //DataAccess.SaveProduct(Product);
        }

        private void btnChoicePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (fileDialog.ShowDialog().Value)
            {
                Product.Photo = File.ReadAllBytes(fileDialog.FileName);
                imageProduct.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void lvCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var countries = lvCountries.Items.Cast<Country>().ToList();
            var productCountry = lvCountries.SelectedItem as Country;
            countries.Remove(productCountry);

            lvCountries.ItemsSource = countries;
        }

        private void cbCounties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var countries = lvCountries.Items.Cast<Country>().ToList();
            var productCountry = cbCounties.SelectedItem as Country;

            if (countries.Where(c => c.Name == productCountry.Name).Count() != 0)
                return;
            countries.Add(productCountry);

            lvCountries.ItemsSource = countries;
        }
    }
}