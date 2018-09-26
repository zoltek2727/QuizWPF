using QuizWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace QuizWPF.Views
{
    public partial class StartWindow : UserControl
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new QuizViewModel();
        }

        private void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new StatsViewModel();
        }
    }
}
