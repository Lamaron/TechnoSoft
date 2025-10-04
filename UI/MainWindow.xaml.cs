using System.Windows;
using Data.Interfaces;
using Data.InMemory;

namespace UI
{
    public partial class MainWindow : Window
    {
        private readonly IRequestRepository _repository;

        public MainWindow()
        {
            InitializeComponent();
            _repository = new RequestRepository(); // Создаем репозиторий
        }

        private void BtnListRequests_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new RepairRequestListWindow(_repository);
            listWindow.Owner = this;
            listWindow.ShowDialog();
        }

        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new Window1(_repository);
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow(_repository);
            statsWindow.Owner = this;
            statsWindow.ShowDialog();
        }
    }
}