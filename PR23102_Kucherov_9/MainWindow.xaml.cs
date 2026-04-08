using PR23102_Kucherov_9.Entity;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PR23102_Kucherov_9
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadAllData();
        }

        private void LoadAllData()
        {
            try
            {
                var teachers = dbHelper.GetAllTeachers();
                LoadData.ItemsSource = teachers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = SearchTextBox.Text.Trim();

                if (string.IsNullOrEmpty(searchText))
                {
                    LoadAllData();
                    return;
                }

                string sortType = (SortComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                var results = dbHelper.SearchTeachers(searchText, sortType);

                if (results.Count == 0)
                {
                    MessageBox.Show("Результаты поиска отсутствуют.", "Информация",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadData.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}