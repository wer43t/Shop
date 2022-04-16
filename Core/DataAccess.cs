using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public static class DataAccess
    {
        public delegate void NewItemAddedDelegate();

        public static event NewItemAddedDelegate NewItemAddedEvent;

        public static ObservableCollection<User> GetUsers()
        {
            return new ObservableCollection<User>(BigShopBase_01Entities.GetContext().User);
        }

        public static User GetUser(int id)
        {
            return GetUsers().Where(user => user.Id == id).FirstOrDefault();
        }

        public static bool IsUserCorrect(string login, string password)
        {
            return GetUser(login, password) != null;
        }

        public static User GetUser(string login, string password)
        {
            return GetUsers().Where(user => user.Login == login && user.Password == password).FirstOrDefault();
        }

        public static bool TryLogin(string login, string password)
        {
            return GetUsers().Where(user => user.Login == login && user.Password == password).Count() == 1;
        }

        public static bool RegistartionUser(string login, string password)
        {
            User user = new User
            {
                Login = login,
                Password = password,
                RoleId = GetRole("Клиент").Id
            };

            BigShopBase_01Entities.GetContext().User.Add(user);
            return Convert.ToBoolean(BigShopBase_01Entities.GetContext().SaveChanges());
        }



        public static bool CheckLogin(string login)
        {
            return GetUsers().Where(user => user.Login == login).Count() == 0;
        }

        public static bool CheckPassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^])(?=.*[^a-zA-Z0-9])\S{6,16}$");

            return regex.IsMatch(password);
        }



        public static ObservableCollection<Role> GetRoles()
        {
            return new ObservableCollection<Role>(BigShopBase_01Entities.GetContext().Role);
        }

        public static Role GetRole(int id)
        {
            return GetRoles().Where(role => role.Id == id).FirstOrDefault();
        }

        public static Role GetRole(string name)
        {
            return GetRoles().Where(role => role.Name == name).FirstOrDefault();
        }


        public static ObservableCollection<Product> GetProducts()
        {
            return new ObservableCollection<Product>(BigShopBase_01Entities.GetContext().Product);
        }

        public static ObservableCollection<Unit> GetUnits()
        {
            return new ObservableCollection<Unit>(BigShopBase_01Entities.GetContext().Unit);
        }

        public static ObservableCollection<Country> GetCountries()
        {
            return new ObservableCollection<Country>(BigShopBase_01Entities.GetContext().Country);
        }

        public static bool DeleteProduct(Product product)
        {
            BigShopBase_01Entities.GetContext().Product.Remove(product);
            return Convert.ToBoolean(BigShopBase_01Entities.GetContext().SaveChanges());
        }

        public static ObservableCollection<ProductCountry> GetProductCountries()
        {
            return new ObservableCollection<ProductCountry>(BigShopBase_01Entities.GetContext().ProductCountry);
        }

        public static bool SaveProduct(Product product, List<ProductCountry> productCountries)
        {
            if (GetProducts().Where(p => p.Id == product.Id).Count() == 0)
            {
                product.AddDate = DateTime.Now;
                BigShopBase_01Entities.GetContext().Product.Add(product);
            }
            else
                BigShopBase_01Entities.GetContext().Product.SingleOrDefault(p => p.Id == product.Id);
            NewItemAddedEvent?.Invoke();
            return Convert.ToBoolean(BigShopBase_01Entities.GetContext().SaveChanges());
        }

        public static ObservableCollection<ProductIntake> GetIntakes()
        {
            return new ObservableCollection<ProductIntake>(BigShopBase_01Entities.GetContext().ProductIntake);
        }
        public static ObservableCollection<Supplier> GetSuppliers()
        {
            return new ObservableCollection<Supplier>(BigShopBase_01Entities.GetContext().Supplier);
        }

        public static void SaveProduct(Product product)
        {
            if (GetProducts().Where(p => p.Id == product.Id).Count() == 0)
            {
                product.AddDate = DateTime.Now;
                BigShopBase_01Entities.GetContext().Product.Add(product);
            }
            else
                BigShopBase_01Entities.GetContext().Product.SingleOrDefault(p => p.Id == product.Id);

            Convert.ToBoolean(BigShopBase_01Entities.GetContext().SaveChanges());
            NewItemAddedEvent?.Invoke();

        }

        public static bool SaveProductCountries(int productId, List<Country> countries)
        {
            foreach (var country in countries)
            {
                ProductCountry productCountry = new ProductCountry
                {
                    ProductId = productId,
                    CountryId = country.Id
                };

                if (GetProductCountries().Where(p => p.ProductId == productId && p.CountryId == country.Id).Count() == 0)
                {
                    BigShopBase_01Entities.GetContext().ProductCountry.Add(productCountry);
                }
            }

            return Convert.ToBoolean(BigShopBase_01Entities.GetContext().SaveChanges());
        }

        public static bool RemoveProductCounrtry(int productId, int countryId)
        {
            BigShopBase_01Entities.GetContext().ProductCountry.Remove(GetProductCountry(productId, countryId));
            return Convert.ToBoolean(BigShopBase_01Entities.GetContext().SaveChanges());
        }

        public static ProductCountry GetProductCountry(int productId, int countryId)
        {
            return GetProductCountries().Where(p => p.ProductId == productId && p.CountryId == countryId).FirstOrDefault();
        }


        public static List<ProductCountry> GetProductCountries(Product product)
        {
            return GetProductCountries().Where(p => p.ProductId == product.Id).ToList();
        }

        public static bool CheckContent(string name, string description)
        {
            Regex regex = new Regex(@"^[А-Яа-яA-Za-z\s\-]+$");

            return regex.IsMatch(name) && regex.IsMatch(description);
        }

    }
}
