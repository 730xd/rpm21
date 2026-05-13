using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using rpm21.Models;

namespace rpm21
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AtelierDbContext _db = new AtelierDbContext();
        private User _currentUser;
        private Atelier _selectedAtelier;
        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            if (_currentUser != null)
            {
                txtUserInfo.Text = $"{_currentUser.FullName} ({_currentUser.Role})";
                txtStatus.Text = $"Добро пожаловать, {_currentUser.FullName}!";

                bool isAdmin = _currentUser.Role == "Admin";
                btnAdd.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                btnEdit.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                btnDelete.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                txtUserInfo.Text = "Гость (просмотр)";
                txtStatus.Text = "Вы вошли как гость - только просмотр";
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }

            LoadFilters();
            LoadData();
        }
        private void LoadFilters()
        {
            cbFilter.Items.Clear();
            cbFilter.Items.Add("Все ателье");
            foreach (var atelier in _db.Ateliers.OrderBy(a => a.Name).ToList())
                cbFilter.Items.Add(atelier.Name);
            cbFilter.SelectedIndex = 0;
        }

        private void LoadData()
        {
            try
            {
                var query = _db.Ateliers.AsQueryable();

                // Поиск
                string search = txtSearch.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(a => a.Name.ToLower().Contains(search) ||
                                             a.Address.ToLower().Contains(search) ||
                                             a.Phone.Contains(search));
                    txtStatus.Text = $"Найдено ателье: {query.Count()}";
                }

                // Фильтр
                if (cbFilter.SelectedIndex > 0)
                {
                    string filterName = cbFilter.SelectedItem.ToString();
                    query = query.Where(a => a.Name == filterName);
                    txtStatus.Text = $"Отфильтровано: {query.Count()} ателье";
                }

                lvAteliers.ItemsSource = query.OrderBy(a => a.Name).ToList();

                if (!query.Any() && !string.IsNullOrEmpty(search))
                    txtStatus.Text = "Ничего не найдено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e) => LoadData();
        private void CbFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) => LoadData();

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            cbFilter.SelectedIndex = 0;
            LoadData();
        }

        private void LvAteliers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedAtelier = lvAteliers.SelectedItem as Atelier;
            if (_selectedAtelier != null)
                txtStatus.Text = $"Выбрано: {_selectedAtelier.Name}";
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new LoginWindow().Show();
                Close();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AtelierDialog(_db);
            if (dialog.ShowDialog() == true)
            {
                LoadFilters();
                LoadData();
                txtStatus.Text = "Ателье добавлено";
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAtelier == null)
            {
                MessageBox.Show("Выберите ателье");
                return;
            }

            var dialog = new AtelierDialog(_db, _selectedAtelier);
            if (dialog.ShowDialog() == true)
            {
                LoadFilters();
                LoadData();
                txtStatus.Text = "Ателье обновлено";
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAtelier == null)
            {
                MessageBox.Show("Выберите ателье");
                return;
            }

            if (MessageBox.Show($"Удалить '{_selectedAtelier.Name}'? Все услуги будут удалены!",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _db.Ateliers.Remove(_selectedAtelier);
                _db.SaveChanges();
                LoadFilters();
                LoadData();
                txtStatus.Text = "Ателье удалено";
            }
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var atelier = button?.Tag as Atelier;

            if (atelier != null)
            {
                var servicesWindow = new ServicesWindow(_db, atelier, _currentUser);
                servicesWindow.ShowDialog();
            }
        }

    }
}