using QuizWPF.Commands;
using QuizWPF.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace QuizWPF.ViewModels
{
    class SubmitViewModel : INotifyPropertyChanged
    {
        QuizDatabaseEntities dbContext = new QuizDatabaseEntities();

        public int PointsShow { get; set; }
        public SubmitViewModel(int points)
        {
            PointsShow = points;
            ExecuteCommand = new CommandHandler(Execute, () => true);
        }

        public ICommand ExecuteCommand { get; }

        private void Execute(object parameter)
        {
            Stats s = new Stats();
            if (Name=="" || Name==null)
            {
                s.Player_Name = "Bezimienny";
            }
            else
            {
                s.Player_Name = Name;
            }
            s.Player_Points = PointsShow;
            s.Game_Played = DateTime.Now;

            dbContext.Stats.Add(s);
            dbContext.SaveChanges();

            Application.Current.MainWindow.DataContext = new EndViewModel(PointsShow);
        }

        public String _Name;
        public String Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
