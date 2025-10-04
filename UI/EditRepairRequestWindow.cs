using System;
using System.Windows;
using System.Windows.Controls;
using Data.Interfaces;
using Data.InMemory;
using Domain;

namespace UI
{
    public partial class Window1 : Window
    {
        private readonly IRequestRepository _repository;
        private Request _currentRequest;

        public bool? Result { get; private set; }
        public Request SavedRequest { get; private set; }
        public bool IsReadOnly { get; set; }

        public Window1(IRequestRepository repository, Request request = null)
        {
            InitializeComponent();
            _repository = repository;
            _currentRequest = request;
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            if (_currentRequest != null)
            {
                Title = "Редактирование заявки";
                FillFormData();
            }
            else
            {
                Title = "Добавление новой заявки";
                dpDateAdded.SelectedDate = DateTime.Now;
                cmbStatus.SelectedIndex = 0;
            }

            SetReadOnlyMode(IsReadOnly);
        }

        private void SetReadOnlyMode(bool readOnly)
        {
            txtNumber.IsReadOnly = readOnly;
            dpDateAdded.IsEnabled = !readOnly;
            cmbEquipmentType.IsEnabled = !readOnly;
            txtEquipmentModel.IsReadOnly = readOnly;
            txtProblemDescription.IsReadOnly = readOnly;
            txtClientFullName.IsReadOnly = readOnly;
            txtClientPhone.IsReadOnly = readOnly;
            cmbStatus.IsEnabled = !readOnly;
            txtResponsibleEngineer.IsReadOnly = readOnly;
            txtComments.IsReadOnly = readOnly;

            if (readOnly)
            {
                var saveButton = FindName("BtnSave") as Button;
                if (saveButton != null)
                    saveButton.Visibility = Visibility.Collapsed;

                var cancelButton = FindName("BtnCancel") as Button;
                if (cancelButton != null)
                    cancelButton.Content = "Закрыть";
            }
        }

        private void FillFormData()
        {
            if (_currentRequest == null) return;

            txtNumber.Text = _currentRequest.Number;
            dpDateAdded.SelectedDate = _currentRequest.Date;
            cmbEquipmentType.Text = _currentRequest.EquipmentType;
            txtEquipmentModel.Text = _currentRequest.EquipmentModel;
            txtProblemDescription.Text = _currentRequest.ProblemDescription;
            txtClientFullName.Text = _currentRequest.ClientFullName;
            txtClientPhone.Text = _currentRequest.ClientPhone;
            cmbStatus.Text = _currentRequest.Status;
            txtResponsibleEngineer.Text = _currentRequest.Engineer;
            txtComments.Text = _currentRequest.Comments;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var request = new Request
                {
                    Number = txtNumber.Text.Trim(),
                    Date = dpDateAdded.SelectedDate ?? DateTime.Now,
                    EquipmentType = cmbEquipmentType.Text,
                    EquipmentModel = txtEquipmentModel.Text.Trim(),
                    ProblemDescription = txtProblemDescription.Text.Trim(),
                    ClientFullName = txtClientFullName.Text.Trim(),
                    ClientPhone = txtClientPhone.Text.Trim(),
                    Status = cmbStatus.Text,
                    Engineer = txtResponsibleEngineer.Text.Trim(),
                    Comments = txtComments.Text.Trim()
                };

                if (_currentRequest != null)
                {
                    // Редактирование существующей заявки
                    request.Id = _currentRequest.Id;
                    _repository.Update(request);
                }
                else
                {
                    // Создание новой заявки
                    _repository.Add(request);
                }

                SavedRequest = request;
                Result = true;

                MessageBox.Show("Заявка успешно сохранена!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                MessageBox.Show("Введите номер заявки", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtClientFullName.Text))
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                txtClientFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtClientPhone.Text))
            {
                MessageBox.Show("Введите номер телефона", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                txtClientPhone.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить изменения и закрыть форму?", "Подтверждение",
                              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Result = false;
                this.Close();
            }
        }
    }
}