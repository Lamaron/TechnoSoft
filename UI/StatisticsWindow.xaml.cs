using Data.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows;
using UI.Helpers;
using static Domain.Request;
using Services;

namespace UI
{
    public partial class StatisticsWindow : Window
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsWindow(StatisticsService statisticsService)
        {
            InitializeComponent();
            _statisticsService = statisticsService;
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var filter = CreateFilter();
            RefreshGeneralStatistics(filter);
            LoadStatusChart(filter);
            LoadMonthChart(filter);
            LoadEngineerChart(filter);
            LoadEquipmentChart(filter);
        }

        private RequestFilter CreateFilter()
        {
            return new RequestFilter
            {
                StartDate = dpStartDate.SelectedDate,
                EndDate = dpEndDate.SelectedDate
            };
        }

        private void RefreshGeneralStatistics(RequestFilter filter)
        {
            var requests = _statisticsService.GetRequestsByStatus(filter);
            var total = requests.Sum(r => r.Count);

            txtTotalRequests.Text = total.ToString();
            txtCompletedRequests.Text = requests
                .Where(r => r.Status == RequestStatus.Completed)
                .Sum(r => r.Count)
                .ToString();
            txtInProgressRequests.Text = requests
                .Where(r => r.Status == RequestStatus.InProgress || r.Status == RequestStatus.WaitingParts)
                .Sum(r => r.Count)
                .ToString();
            txtNewRequests.Text = requests
                .Where(r => r.Status == RequestStatus.New)
                .Sum(r => r.Count)
                .ToString();
        }

        private void LoadStatusChart(RequestFilter filter)
        {
            var data = _statisticsService.GetRequestsByStatus(filter);
            var plotModel = new PlotModel { Title = "Распределение заявок по статусам" };

            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            foreach (var item in data)
            {
                var label = StatusHelper.GetDisplayText(item.Status);
                pieSeries.Slices.Add(new PieSlice(label, item.Count)
                {
                    IsExploded = false
                });
            }

            plotModel.Series.Add(pieSeries);
            plotStatus.Model = plotModel;
        }

        private void LoadMonthChart(RequestFilter filter)
        {
            var data = _statisticsService.GetRequestsByMonth(filter);
            var plotModel = new PlotModel { Title = "Динамика заявок по месяцам" };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Angle = -15,
                Title = "Месяцы"
            };

            foreach (var item in data)
            {
                categoryAxis.Labels.Add(item.GetMonthName());
            }

            plotModel.Axes.Add(categoryAxis);

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Количество заявок",
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            var lineSeries = new LineSeries
            {
                Title = "Количество заявок",
                Color = OxyColor.FromRgb(79, 129, 189),
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerFill = OxyColor.FromRgb(79, 129, 189)
            };

            for (int i = 0; i < data.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, data[i].Count));
            }

            plotModel.Series.Add(lineSeries);
            plotMonth.Model = plotModel;
        }

        private void LoadEngineerChart(RequestFilter filter)
        {
            var data = _statisticsService.GetRequestsByEngineer(filter);
            var plotModel = new PlotModel { Title = "Статистика по инженерам" };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Инженеры"
            };

            foreach (var item in data)
            {
                categoryAxis.Labels.Add(item.EngineerName);
            }

            plotModel.Axes.Add(categoryAxis);

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Количество заявок",
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            var barSeries = new BarSeries
            {
                Title = "Всего заявок",
                FillColor = OxyColor.FromRgb(79, 129, 189)
            };

            foreach (var item in data)
            {
                barSeries.Items.Add(new BarItem { Value = item.Count });
            }

            plotModel.Series.Add(barSeries);
            plotEngineer.Model = plotModel;
        }

        private void LoadEquipmentChart(RequestFilter filter)
        {
            var data = _statisticsService.GetRequestsByEquipment(filter);
            var plotModel = new PlotModel { Title = "Статистика по типам оборудования" };

            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            foreach (var item in data)
            {
                pieSeries.Slices.Add(new PieSlice(item.EquipmentType, item.Count)
                {
                    IsExploded = false
                });
            }

            plotModel.Series.Add(pieSeries);
            plotEquipment.Model = plotModel;
        }

        private void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
        }

        private void BtnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
            LoadStatistics();
        }

        private void BtnRefreshStats_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
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