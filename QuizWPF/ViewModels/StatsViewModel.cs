using QuizWPF.Models;
using System.Linq;
using System.Collections.Generic;

namespace QuizWPF.ViewModels
{
    class StatsViewModel
    {
        QuizDatabaseEntities dbContext = new QuizDatabaseEntities();

        public StatsViewModel()
        {
            List<Stats> allStats = dbContext.Stats.OrderByDescending(x => x.Player_Points).ToList();
            List<Results> tempStats = new List<Results>();

            int i = 1;
            foreach(Stats s in allStats)
            {
                Results r = new Results();
                r.id = i;
                r.Name = s.Player_Name;
                r.points = s.Player_Points;
                r.id_Stat = s.Id_Stat;
                tempStats.Add(r);

                i++;
            }

            ListViewStats = tempStats;
        }

        public List<Results> ListViewStats { get; set; }
    }
}
