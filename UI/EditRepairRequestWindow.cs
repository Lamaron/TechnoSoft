using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Data.Interfaces;
using Data.InMemory;
using Domain;

namespace UI
{
    public partial class Window1 : Window
    {
        private IRequestRepository _repository;
        private Request _currentRequest;

        public bool? Result { get; private set; }
        public Request SavedRequest { get; private set; }
        public bool IsReadOnly { get; set; }

        // Конструктор с параметром
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
                Title = IsReadOnly ? "Просмотр заявки" : "Редактирование заявки";
                FillFormData();
            }
            else
            {
                Title = "Добавление новой заявки";
                dpDateAdded.SelectedDate = DateTime.Now;
                cmbStatus.SelectedIndex = 0;

                // Автогенерация номера заявки
                if (_repository != null)
                {
                    txtNumber.Text = _repository.GenerateRequestNumber();
                }
                else
                {
                    txtNumber.Text = "REQ-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
                }
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
                BtnSave.Visibility = Visibility.Collapsed;
                BtnCancel.Content = "Закрыть";
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
            if (!ValidateForm()) // Этот метод должен быть ниже
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
                    if (!_repository.Update(request))
                    {
                        MessageBox.Show("Не удалось обновить заявку. Возможно, номер заявки уже существует.", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    // Создание новой заявки
                    try
                    {
                        _repository.Add(request);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
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

        // ДОБАВЛЯЕМ ПРОПУЩЕННЫЙ МЕТОД ValidateForm
        private bool ValidateForm()
        {
            // Проверка номера заявки
            if (string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                ShowValidationError("Введите номер заявки", txtNumber);
                return false;
            }

            // Проверка даты
            if (dpDateAdded.SelectedDate == null)
            {
                ShowValidationError("Выберите дату добавления", dpDateAdded);
                return false;
            }

            // Проверка типа оборудования
            if (string.IsNullOrWhiteSpace(cmbEquipmentType.Text))
            {
                ShowValidationError("Выберите тип оборудования", cmbEquipmentType);
                return false;
            }

            // Проверка модели оборудования
            if (string.IsNullOrWhiteSpace(txtEquipmentModel.Text))
            {
                ShowValidationError("Введите модель оборудования", txtEquipmentModel);
                return false;
            }

            // Проверка описания проблемы
            if (string.IsNullOrWhiteSpace(txtProblemDescription.Text))
            {
                ShowValidationError("Введите описание проблемы", txtProblemDescription);
                return false;
            }

            // Проверка ФИО клиента
            if (string.IsNullOrWhiteSpace(txtClientFullName.Text))
            {
                ShowValidationError("Введите ФИО клиента", txtClientFullName);
                return false;
            }

            // Проверка телефона
            if (string.IsNullOrWhiteSpace(txtClientPhone.Text))
            {
                ShowValidationError("Введите номер телефона", txtClientPhone);
                return false;
            }

            if (!IsValidPhone(txtClientPhone.Text))
            {
                ShowValidationError("Введите корректный номер телефона", txtClientPhone);
                return false;
            }

            // Проверка статуса
            if (string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                ShowValidationError("Выберите статус заявки", cmbStatus);
                return false;
            }

            return true;
        }

        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Ошибка валидации",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
        }

        private bool IsValidPhone(string phone)
        {
            // Упрощенная проверка телефона - минимум 10 цифр
            var digitsOnly = Regex.Replace(phone, @"\D", "");
            return digitsOnly.Length >= 10;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!IsReadOnly && HasChanges())
            {
                var result = MessageBox.Show("Есть несохраненные изменения. Закрыть форму?", "Подтверждение",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                    return;
            }

            Result = false;
            this.Close();
        }

        private bool HasChanges()
        {
            if (_currentRequest == null)
            {
                // Для новой заявки проверяем, есть ли введенные данные
                return !string.IsNullOrWhiteSpace(txtNumber.Text) ||
                       !string.IsNullOrWhiteSpace(txtClientFullName.Text) ||
                       !string.IsNullOrWhiteSpace(txtClientPhone.Text);
            }

            // Для существующей заявки проверяем изменения
            return txtNumber.Text != _currentRequest.Number ||
                   dpDateAdded.SelectedDate != _currentRequest.Date ||
                   cmbEquipmentType.Text != _currentRequest.EquipmentType ||
                   txtEquipmentModel.Text != _currentRequest.EquipmentModel ||
                   txtProblemDescription.Text != _currentRequest.ProblemDescription ||
                   txtClientFullName.Text != _currentRequest.ClientFullName ||
                   txtClientPhone.Text != _currentRequest.ClientPhone ||
                   cmbStatus.Text != _currentRequest.Status ||
                   txtResponsibleEngineer.Text != _currentRequest.Engineer ||
                   txtComments.Text != _currentRequest.Comments;
        }
    }
}