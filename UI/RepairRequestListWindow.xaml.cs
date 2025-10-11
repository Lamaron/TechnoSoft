using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Data.Interfaces;
using Data.InMemory;
using Domain;
using UI.Helpers;

namespace UI
{
    public partial class RepairRequestListWindow : Window
    {
        private IRequestRepository _repository;
        private Request _selectedRequest;

        public RepairRequestListWindow()
        {
            InitializeComponent();
            _repository = new RequestRepository();
            InitializeStatusFilter();
            Loaded += RepairRequestListWindow_Loaded;
            UpdateActionButtons();
        }

        private void InitializeStatusFilter()
        {
            cmbStatusFilter.Items.Add(new ComboBoxItem { Content = "Все статусы", IsSelected = true });

            foreach (var statusItem in StatusHelper.GetStatusItems())
            {
                cmbStatusFilter.Items.Add(new ComboBoxItem { Content = statusItem.DisplayText });
            }
        }

        public RepairRequestListWindow(IRequestRepository repository) : this()
        {
            _repository = repository; 
        }

        private void RepairRequestListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshRequestsList();
        }

        private void RefreshRequestsList()
        {
            try
            {
                var allRequests = _repository.GetAll();

                if (allRequests == null)
                {
                    dgRequests.ItemsSource = null;
                    txtRecordCount.Text = "Записей: 0";
                    return;
                }

                var filteredRequests = allRequests;

                if (cmbStatusFilter.SelectedItem is ComboBoxItem statusItem &&
                    statusItem.Content.ToString() != "Все статусы")
                {
                    var selectedStatus = StatusHelper.GetStatusByDisplayText(statusItem.Content.ToString());
                    filteredRequests = filteredRequests.Where(r => r.Status == selectedStatus).ToList();
                }

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
            }
        }

        private void UpdateActionButtons()
        {
            bool hasSelection = _selectedRequest != null;
            btnEdit.IsEnabled = hasSelection;
            btnDelete.IsEnabled = hasSelection;
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