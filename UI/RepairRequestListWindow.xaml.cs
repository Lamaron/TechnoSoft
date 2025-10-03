using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Data.Interfaces;
using Data.InMemory;
using Domain;

namespace UI
{
    public partial class RepairRequestListWindow : Window
    {
        private IRequestRepository _repository;
        private Request _selectedRequest;

        // Конструктор БЕЗ параметров (для XAML)
        public RepairRequestListWindow()
        {
            InitializeComponent();
            _repository = new RequestRepository(); // Создаем репозиторий здесь
            Loaded += RepairRequestListWindow_Loaded;
            UpdateActionButtons();
        }

        // Конструктор с параметром (для кода)
        public RepairRequestListWindow(IRequestRepository repository) : this()
        {
            _repository = repository; // Используем переданный репозиторий
        }

        private void RepairRequestListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshRequestsList();
        }

        private void RefreshRequestsList()
        {
            try
            {
                if (_repository == null)
                {
                    MessageBox.Show("Репозиторий не инициализирован", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var allRequests = _repository.GetAll();

                if (allRequests == null)
                {
                    dgRequests.ItemsSource = null;
                    txtRecordCount.Text = "Записей: 0";
                    return;
                }

                var filteredRequests = allRequests;

                // Фильтрация по статусу
                if (cmbStatusFilter.SelectedItem != null)
                {
                    var selectedStatus = (cmbStatusFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Все статусы")
                    {
                        filteredRequests = filteredRequests.Where(r => r.Status == selectedStatus).ToList();
                    }
                }

                // Поиск
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    var searchText = txtSearch.Text.ToLower();
                    filteredRequests = filteredRequests.Where(r =>
                        (r.Number != null && r.Number.ToLower().Contains(searchText)) ||
                        (r.ClientFullName != null && r.ClientFullName.ToLower().Contains(searchText)) ||
                        (r.EquipmentType != null && r.EquipmentType.ToLower().Contains(searchText)) ||
                        (r.EquipmentModel != null && r.EquipmentModel.ToLower().Contains(searchText)) ||
                        (r.Engineer != null && r.Engineer.ToLower().Contains(searchText))
                    ).ToList();
                }

                dgRequests.ItemsSource = filteredRequests;
                txtRecordCount.Text = $"Записей: {filteredRequests.Count}";
                UpdateActionButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateActionButtons()
        {
            bool hasSelection = _selectedRequest != null;
            btnEdit.IsEnabled = hasSelection;
            btnDelete.IsEnabled = hasSelection;
            btnView.IsEnabled = hasSelection;
        }

        private void BtnCreateNewRequest_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new Window1(_repository);
            editWindow.Owner = this;
            if (editWindow.ShowDialog() == true)
            {
                RefreshRequestsList();
                txtStatus.Text = "Новая заявка создана";
            }
        }

        private void BtnEditRequest_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest != null)
            {
                EditSelectedRequest();
            }
        }

        private void BtnDeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить заявку {_selectedRequest.Number}?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_repository.Delete(_selectedRequest.Id))
                    {
                        _selectedRequest = null;
                        RefreshRequestsList();
                        txtStatus.Text = "Заявка удалена";
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить заявку", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnViewRequest_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest != null)
            {
                var viewWindow = new Window1(_repository, _selectedRequest);
                viewWindow.Title = "Просмотр заявки";
                viewWindow.Owner = this;
                viewWindow.IsReadOnly = true;
                viewWindow.ShowDialog();
            }
        }

        private void EditSelectedRequest()
        {
            var editWindow = new Window1(_repository, _selectedRequest);
            editWindow.Owner = this;
            if (editWindow.ShowDialog() == true)
            {
                RefreshRequestsList();
                txtStatus.Text = "Заявка обновлена";
            }
        }

        private void DgRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRequest = dgRequests.SelectedItem as Request;
            UpdateActionButtons();
        }

        private void DgRequests_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_selectedRequest != null)
            {
                EditSelectedRequest();
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            RefreshRequestsList();
            txtStatus.Text = "Поиск выполнен";
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshRequestsList();
            txtStatus.Text = "Список обновлен";
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshRequestsList();
        }

        private void CmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshRequestsList();
        }
    }
}