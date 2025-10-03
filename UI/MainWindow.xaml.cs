using System.Windows;
using Data.InMemory;

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
            // Теперь используем конструктор без параметров
            var listWindow = new RepairRequestListWindow();
            listWindow.Owner = this;
            listWindow.ShowDialog();
        }

        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new Window1(new RequestRepository());
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow(new RequestRepository());
            statsWindow.Owner = this;
            statsWindow.ShowDialog();
        }
    }
}