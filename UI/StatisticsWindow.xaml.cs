using System.Linq;
using System.Windows;
using Data.Interfaces;
using Domain;

namespace UI
{
    public partial class StatisticsWindow : Window
    {
        private readonly IRequestRepository _repository;

        public StatisticsWindow(IRequestRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            RefreshStatistics();
        }

        private void RefreshStatistics()
        {
            var requests = _repository.GetAll();

            if (requests == null || !requests.Any())
            {
                ClearStatistics();
                return;
            }

            // Общая статистика
            txtTotalRequests.Text = requests.Count.ToString();
            txtCompletedRequests.Text = requests.Count(r => r.Status == "Завершена").ToString();
            txtInProgressRequests.Text = requests.Count(r => r.Status == "В процессе ремонта" || r.Status == "Ожидание запчастей").ToString();
            txtNewRequests.Text = requests.Count(r => r.Status == "Новая заявка").ToString();

            // Статистика по типам оборудования
            var equipmentStats = requests
                .GroupBy(r => r.EquipmentType)
                .Select(g => new
                {
                    Key = string.IsNullOrEmpty(g.Key) ? "Не указан" : g.Key,
                    Value = g.Count(),
                    Percentage = $"{g.Count() * 100.0 / requests.Count:F1}%"
                })
                .OrderByDescending(x => x.Value)
                .ToList();
            dgEquipmentStats.ItemsSource = equipmentStats;

            // Статистика по статусам
            var statusStats = requests
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                    Key = string.IsNullOrEmpty(g.Key) ? "Не указан" : g.Key,
                    Value = g.Count(),
                    Percentage = $"{g.Count() * 100.0 / requests.Count:F1}%"
                })
                .OrderByDescending(x => x.Value)
                .ToList();
            dgStatusStats.ItemsSource = statusStats;

            // Статистика по инженерам
            var engineerStats = requests
                .GroupBy(r => string.IsNullOrEmpty(r.Engineer) ? "Не назначен" : r.Engineer)
                .Select(g => new
                {
                    Key = g.Key,
                    Value = g.Count(),
                    Completed = g.Count(r => r.Status == "Завершена")
                })
                .OrderByDescending(x => x.Value)
                .ToList();
            dgEngineerStats.ItemsSource = engineerStats;
        }

        private void ClearStatistics()
        {
            txtTotalRequests.Text = "0";
            txtCompletedRequests.Text = "0";
            txtInProgressRequests.Text = "0";
            txtNewRequests.Text = "0";

            dgEquipmentStats.ItemsSource = null;
            dgStatusStats.ItemsSource = null;
            dgEngineerStats.ItemsSource = null;
        }

        private void BtnRefreshStats_Click(object sender, RoutedEventArgs e)
        {
            RefreshStatistics();
            MessageBox.Show("Статистика обновлена", "Обновление",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция экспорта в PDF будет реализована в будущей версии",
                          "В разработке",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}