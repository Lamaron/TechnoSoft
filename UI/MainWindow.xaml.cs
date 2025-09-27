using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnListRequests_Click(object sender, RoutedEventArgs e)
        {
            // Пока просто показываем сообщение
            MessageBox.Show("Функционал списка заявок в разработке", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new Window1();
            editWindow.Show();
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал статистики в разработке", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}