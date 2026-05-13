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
    /// Логика взаимодействия для ServicePriceDialog.xaml
    /// </summary>
    public partial class ServicePriceDialog : Window
    {
        private AtelierDbContext _db;
        private int _atelierId;
        private ServicePrice _editItem;
        public ServicePriceDialog(AtelierDbContext db, int atelierId, ServicePrice item = null)
        {
            InitializeComponent();
            _db = db;
            _atelierId = atelierId;
            _editItem = item;
            cbService.ItemsSource = _db.Services.ToList();

            if (item != null)
            {
                lblTitle.Text = "Редактирование цены";
                cbService.SelectedValue = item.ServiceId;
                txtPrice.Text = item.Price.ToString();
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbService.SelectedItem == null || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (_editItem != null)
            {
                _editItem.Price = decimal.Parse(txtPrice.Text);
            }
            else
            {
                var servicePrice = new ServicePrice
                {
                    AtelierId = _atelierId,
                    ServiceId = (int)cbService.SelectedValue,
                    Price = decimal.Parse(txtPrice.Text)
                };
                _db.ServicePrices.Add(servicePrice);
            }

            _db.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

