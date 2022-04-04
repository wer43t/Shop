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
        public static ObservableCollection<User> GetUsers()
        {
            return new ObservableCollection<User>(BigShopBase_01Entities.GetContext().User);
        }

        public static User GetUser(int id)
        {
            return GetUsers().Where(user => user.Id == id).FirstOrDefault();
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
    }
}
