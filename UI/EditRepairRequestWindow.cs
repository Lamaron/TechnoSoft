using System;
using System.Windows;
using System.Windows.Controls;
using Domain;

namespace UI
{
    public partial class Window1 : Window
    {
        private Request _currentRequest;

        public Window1(Request request = null)
        {
            InitializeComponent();
            _currentRequest = request;

            // Если передан существующий объект - заполняем форму
            if (_currentRequest != null)
            {
                Title = "Редактирование заявки";
                FillFormData();
            }
            else
            {
                Title = "Добавление новой заявки";
                dpDateAdded.SelectedDate = DateTime.Now;
                cmbStatus.SelectedIndex = 0; // Новая заявка
            }
        }

        private void FillFormData()
        {
            txtNumber.Text = _currentRequest.Id;
            dpDateAdded.SelectedDate = _currentRequest.Date;
            cmbEquipmentType.Text = _currentRequest.Tipe;
            txtEquipmentModel.Text = _currentRequest.Model;
            txtProblemDescription.Text = _currentRequest.Description;
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
                // Создаем объект через конструктор по умолчанию
                var request = new Request
                {
                    Id = txtNumber.Text.Trim(),
                    Date = dpDateAdded.SelectedDate ?? DateTime.Now,
                    Tipe = cmbEquipmentType.Text, // Просто берем Text
                    Model = txtEquipmentModel.Text.Trim(),
                    Description = txtProblemDescription.Text.Trim(),
                    ClientFullName = txtClientFullName.Text.Trim(),
                    ClientPhone = txtClientPhone.Text.Trim(),
                    Status = cmbStatus.Text, // Просто берем Text
                    Engineer = txtResponsibleEngineer.Text.Trim(),
                    Comments = txtComments.Text.Trim()
                };

                // Если редактируем существующую заявку, сохраняем оригинальный Id
                if (_currentRequest != null)
                    request.Id = _currentRequest.Id;

                // Здесь будет вызов репозитория для сохранения
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
                this.Close();
            }
        }
    }
}