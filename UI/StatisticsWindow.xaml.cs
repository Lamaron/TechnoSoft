using Data.Interfaces;
using Domain;
using System.Linq;
using System.Windows;
using UI.Helpers;
using static Domain.Request;

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

            txtTotalRequests.Text = requests.Count.ToString();
            txtCompletedRequests.Text = requests.Count(r => r.Status == RequestStatus.Completed).ToString();
            txtInProgressRequests.Text = requests.Count(r => r.Status == RequestStatus.InProgress ||
                                                           r.Status == RequestStatus.WaitingParts).ToString();
            txtNewRequests.Text = requests.Count(r => r.Status == RequestStatus.New).ToString();


            var equipmentStats = requests
                .GroupBy(r => r.EquipmentType)
                .Select(g => new
                {
                    Value = g.Count(),
                    Percentage = $"{g.Count() * 100.0 / requests.Count:F1}%"
                })
                .OrderByDescending(x => x.Value)
                .ToList();
            dgEquipmentStats.ItemsSource = equipmentStats;

            var statusStats = requests
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                    Value = g.Count(),
                    Percentage = $"{g.Count() * 100.0 / requests.Count:F1}%"
                })
                .OrderByDescending(x => x.Value)
                .ToList();
            dgStatusStats.ItemsSource = statusStats;

            var engineerStats = requests
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                Key = StatusHelper.GetDisplayText(g.Key),
                Value = g.Count(),
                Percentage = $"{g.Count() * 100.0 / requests.Count:F1}%"
                })
                .OrderByDescending(x => x.Value)
                .ToList();
                dgStatusStats.ItemsSource = statusStats;
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