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
    /// Логика взаимодействия для AtelierDialog.xaml
    /// </summary>
    public partial class AtelierDialog : Window
    {
        private AtelierDbContext _db;
        private Atelier _editItem;
        private bool _isEdit;
        public AtelierDialog(AtelierDbContext db, Atelier item = null)
        {
            InitializeComponent();
            _db = db;
            _editItem = item;
            _isEdit = item != null;

            if (_isEdit)
            {
                lblTitle.Text = "Редактирование ателье";
                LoadData();
            }
        }
            private void LoadData()
        {
            txtNumber.Text = _editItem.Number.ToString();
            txtName.Text = _editItem.Name;
            txtAddress.Text = _editItem.Address;
            txtPhone.Text = _editItem.Phone;
            txtPhotoPath.Text = _editItem.PhotoPath;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название");
                return;
            }

            if (_isEdit)
            {
                _editItem.Number = int.Parse(txtNumber.Text);
                _editItem.Name = txtName.Text;
                _editItem.Address = txtAddress.Text;
                _editItem.Phone = txtPhone.Text;
                _editItem.PhotoPath = txtPhotoPath.Text;
            }
            else
            {
                var atelier = new Atelier
                {
                    Number = int.Parse(txtNumber.Text),
                    Name = txtName.Text,
                    Address = txtAddress.Text,
                    Phone = txtPhone.Text,
                    PhotoPath = txtPhotoPath.Text
                };
                _db.Ateliers.Add(atelier);
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

