using Data.Interfaces;
using Services;
using System.Windows;

namespace UI
{
    public partial class MainWindow : Window
    {
        private IRequestRepository _requestRepository;

        public MainWindow(IRequestRepository requestRepository)
        {
            InitializeComponent();
            _requestRepository = requestRepository;
        }


        private void BtnListRequests_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new RepairRequestListWindow(_requestRepository);
            listWindow.Owner = this;
            listWindow.ShowDialog();
        }

        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new Window1(_requestRepository);
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            var statisticsService = new StatisticsService(_requestRepository);
            var statsWindow = new StatisticsWindow(statisticsService);
            statsWindow.Owner = this;
            statsWindow.ShowDialog();
        }
    }
}