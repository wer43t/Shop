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
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();

            tbLogin.Text = Properties.Settings.Default.Login;
            Properties.Settings.Default.attemptsCount = 0;
            Properties.Settings.Default.Save();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = tbLogin.Text;
            var password = pbPassword.Password;

            if (DataAccess.IsUserCorrect(login, password) && Properties.Settings.Default.LastLoginAttempt == DateTime.MinValue)
            {
                Properties.Settings.Default.attemptsCount = 0;
                if (cbRemember.IsChecked.GetValueOrDefault())
                    Properties.Settings.Default.Login = login;
                else
                    Properties.Settings.Default.Login = null;
                Properties.Settings.Default.LastLoginAttempt = DateTime.MinValue;
                Properties.Settings.Default.Save();
                NavigationService.Navigate(new ProductsPage());
            }
            else if (Properties.Settings.Default.attemptsCount == 2 || Properties.Settings.Default.LastLoginAttempt != DateTime.MinValue)
            {
                if (Properties.Settings.Default.LastLoginAttempt == DateTime.MinValue)
                {
                    Properties.Settings.Default.LastLoginAttempt = DateTime.Now;
                    MessageBox.Show("Ты поставлен на счетчик, у тебя 1 минута", "Ошибка");
                    Properties.Settings.Default.Save();
                }
                else if (DateTime.Now - Properties.Settings.Default.LastLoginAttempt >= TimeSpan.FromMinutes(1))
                {
                    Properties.Settings.Default.attemptsCount = 0;
                    Properties.Settings.Default.LastLoginAttempt = DateTime.MinValue;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show($"Ты поставлен на счетчик, у тебя {60 - Math.Round((DateTime.Now - Properties.Settings.Default.LastLoginAttempt).TotalSeconds)} сек", "Ошибка");
                }
            }
            else
            {
                Properties.Settings.Default.attemptsCount++;
                Properties.Settings.Default.Save();
                MessageBox.Show("Неверный логин или пароль", "Ошибка");
            }
        }
        private bool IsPasswordCorrect()
        {
            var result = true;
            if (Properties.Settings.Default.LastLoginAttempt != DateTime.MinValue || Properties.Settings.Default.attemptsCount == 2)
            {
                if (!((Properties.Settings.Default.LastLoginAttempt == DateTime.MinValue) != (Properties.Settings.Default.attemptsCount == 2)))
                {
                    Properties.Settings.Default.LastLoginAttempt = DateTime.Now;
                    MessageBox.Show("Ты поставлен на счетчик, у тебя 1 минута", "Ошибка");
                    Properties.Settings.Default.Save();
                    result = false;
                }
                else if (DateTime.Now - Properties.Settings.Default.LastLoginAttempt >= TimeSpan.FromMinutes(1))
                {
                    Properties.Settings.Default.attemptsCount = 0;
                    Properties.Settings.Default.LastLoginAttempt = DateTime.MinValue;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show($"Ты поставлен на счетчик, у тебя {60 - (DateTime.Now - Properties.Settings.Default.LastLoginAttempt).TotalSeconds} сек", "Ошибка");
                    result = false;
                }
            }
            return result;
        }
    }
}
