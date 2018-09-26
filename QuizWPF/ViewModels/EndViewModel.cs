using QuizWPF.Commands;
using QuizWPF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuizWPF.ViewModels
{
    class EndViewModel
    {
        QuizDatabaseEntities dbContext = new QuizDatabaseEntities();

        public int PointsShow { get; set; }
        public EndViewModel(int points)
        {
            PointsShow = points;
            ExecuteCommand = new CommandHandler(Execute, () => true);

            int idLatest = dbContext.Stats.OrderByDescending(x => x.Game_Played).Select(x => x.Id_Stat).First();
            List<Stats> allStats = dbContext.Stats.OrderByDescending(x => x.Player_Points).ToList();
            List<Results> tempStats = new List<Results>();

            int i = 1;
            foreach (Stats s in allStats)
            {
                Results r = new Results();
                r.id = i;
                r.Name = s.Player_Name;
                r.points = s.Player_Points;
                r.id_Stat = s.Id_Stat;
                if (s.Id_Stat == idLatest)
                {
                    r.is_Selected = true;
                }
                else r.is_Selected = false;
                tempStats.Add(r);

                i++;
            }

            ListViewStats = tempStats;
        }

        public List<Results> ListViewStats { get; set; }
        public int Count { get; set; }

        public ICommand ExecuteCommand { get; }
        private void Execute(object parameter)
        {
            PointsShow = 0;
            Application.Current.MainWindow.DataContext = new StartViewModel();
        }
    }
}
