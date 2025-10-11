using System.Windows;
using Data;

namespace UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnListRequests_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new RepairRequestListWindow(RepositoryContainer.RequestRepository);
            listWindow.Owner = this;
            listWindow.ShowDialog();
        }

        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new Window1(RepositoryContainer.RequestRepository);
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow(RepositoryContainer.RequestRepository);
            statsWindow.Owner = this;
            statsWindow.ShowDialog();
        }
    }
}