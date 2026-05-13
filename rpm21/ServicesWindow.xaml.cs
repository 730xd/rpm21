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
    /// Логика взаимодействия для ServicesWindow.xaml
    /// </summary>
    public partial class ServicesWindow : Window
    {
        private AtelierDbContext _db;
        private Atelier _atelier;
        private User _currentUser;
        public ServicesWindow(AtelierDbContext db, Atelier atelier, User user)
        {
            InitializeComponent();
            _db = db;
            _atelier = atelier;
            _currentUser = user;

            txtAtelierName.Text = atelier.Name;
            txtAtelierAddress.Text = "📍 " + atelier.Address;
            txtAtelierPhone.Text = "📞 " + atelier.Phone;

            bool isAdmin = _currentUser != null && _currentUser.Role == "Admin";
            btnAddService.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;

            LoadData();
        }
        private void LoadData()
        {
            var prices = _db.ServicePrices
                .Where(sp => sp.AtelierId == _atelier.Id)
                .Select(sp => new { sp.Id, sp.Service, sp.Price })
                .ToList();

            lvServices.ItemsSource = prices;
            txtStatus.Text = "Всего услуг: " + prices.Count;
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ServicePriceDialog(_db, _atelier.Id);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                LoadData();
                txtStatus.Text = "Услуга добавлена";
            }
        }

        private void BtnEditPrice_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            dynamic item = button?.Tag;
            int priceId = item.Id;

            var servicePrice = _db.ServicePrices.Find(priceId);
            var dialog = new ServicePriceDialog(_db, _atelier.Id, servicePrice);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                LoadData();
                txtStatus.Text = "Цена обновлена";
            }
        }

        private void BtnDeletePrice_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            dynamic item = button?.Tag;
            int priceId = item.Id;
            string serviceName = item.Service.Name;

            var result = MessageBox.Show("Удалить услугу '" + serviceName + "'?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var servicePrice = _db.ServicePrices.Find(priceId);
                _db.ServicePrices.Remove(servicePrice);
                _db.SaveChanges();
                LoadData();
                txtStatus.Text = "Услуга удалена";
            }
        }
    }
}
