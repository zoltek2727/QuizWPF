using QuizWPF.ViewModels;
using System.Windows;

namespace QuizWPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StartViewModel();
        }
    }
}
