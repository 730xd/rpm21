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
using System.Windows.Shapes;
using rpm21.Models;

namespace rpm21
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AtelierDbContext _db = new AtelierDbContext();
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = _db.Users.FirstOrDefault(u => u.Login == txtLogin.Text && u.Password == txtPassword.Password);

            if (user != null)
            {
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                Close();
            }
            else
            {
                txtError.Text = "Неверный логин или пароль!";
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(null);
            mainWindow.Show();
            Close();
        }
    }
}

