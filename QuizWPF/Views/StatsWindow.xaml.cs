using QuizWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace QuizWPF.Views
{
    public partial class StatsWindow : UserControl
    {
        public StatsWindow()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new StartViewModel();
        }
    }
}
