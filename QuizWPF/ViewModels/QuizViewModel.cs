using QuizWPF.Commands;
using QuizWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace QuizWPF.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        QuizDatabaseEntities dbContext = new QuizDatabaseEntities();

        List<Questions> random;

        private BackgroundWorker bWorker;

        private int pointsSum = 0;
        private int iteration = 0;
        private decimal helpPoints = 0;

        private String _HelpPoints;
        public String HelpPoints
        {
            get { return _HelpPoints; }
            set
            {
                _HelpPoints = value;
                OnPropertyChanged("HelpPoints");
            }
        }

        private String _QuestionLabel;
        public String QuestionLabel
        {
            get { return _QuestionLabel; }
            set
            {
                _QuestionLabel = value;
                OnPropertyChanged("QuestionLabel");
            }
        }

        private String _QuestionImage;
        public String QuestionImage
        {
            get { return _QuestionImage; }
            set
            {
                _QuestionImage = value;
                OnPropertyChanged("QuestionImage");
            }
        }

        private List<AnswerQuestionConnection> _AnswerButtons;
        public List<AnswerQuestionConnection> AnswerButtons
        {
            get { return _AnswerButtons; }
            set
            {
                _AnswerButtons = value;
                OnPropertyChanged("AnswerButtons");
            }
        }

        public QuizViewModel()
        {
            random = dbContext.Questions.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            QuestionLabel = random[iteration].Question_Description;

            QuestionImage = random[iteration].Question_Image_Source;

            int idTest = random[iteration].Id_Question;
            List<AnswerQuestionConnection> answers = dbContext.AnswerQuestionConnection.Where(x => x.Id_Question == idTest).ToList();
            AnswerButtons = answers;

            lblWrongBool = false;
            lblRightBool = false;

            ExecuteCommand = new CommandHandler(Execute, () => true);
            StopCommand = new CommandHandler(ExecuteEnd, () => true);

            b_worker_Start();
        }

        void b_worker_Start()
        {
            bWorker = new BackgroundWorker();

            bWorker.WorkerReportsProgress = true;
            bWorker.WorkerSupportsCancellation = true;
            bWorker.DoWork += bWorker_DoWork;
            bWorker.ProgressChanged += bWorker_ProgressChanged;
            bWorker.RunWorkerCompleted += bWorker_RunWorkerCompleted;
            bWorker.RunWorkerAsync(100);
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int max = (int)e.Argument;
            for (int i = 0; i < max; i++)
            {
                if (bWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                int progressPercentage = Convert.ToInt32(((double)i / (max - 1)) * 100);
                (sender as BackgroundWorker).ReportProgress(progressPercentage);

                helpPoints = 10-(i/10);

                Thread.Sleep(100);
            }
        }

        void bWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
            HelpPoints = (Convert.ToInt32(helpPoints)).ToString();
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!stopQuiz)
            {
                Thread.Sleep(500);
                iteration++;
                HelpPoints = "0";
                lblWrongBool = false;
                lblRightBool = false;

                if (random.Count > iteration)
                {
                    b_worker_Start();
                    QuestionLabel = random[iteration].Question_Description;

                    QuestionImage = random[iteration].Question_Image_Source;

                    int idTest = random[iteration].Id_Question;
                    List<AnswerQuestionConnection> answers = dbContext.AnswerQuestionConnection.Where(x => x.Id_Question == idTest).ToList();
                    AnswerButtons = answers;
                }
                else
                {
                    Application.Current.MainWindow.DataContext = new SubmitViewModel(pointsSum);
                }
            }
        }

        private int _CurrentProgress;
        public int CurrentProgress
        {
            get { return _CurrentProgress; }
            private set
            {
                if (_CurrentProgress != value)
                {
                    _CurrentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }

        public ICommand ExecuteCommand { get; }
        private void Execute(object parameter)
        {
            AnswerQuestionConnection aqcCurrent = parameter as AnswerQuestionConnection;
            if (aqcCurrent.Is_Answer_Correct)
            {
                lblRightBool = true;
                pointsSum += Convert.ToInt32(helpPoints);
            }
            else
            {
                lblWrongBool = true;
            }
            if (bWorker.IsBusy)
            {
                bWorker.CancelAsync();
            }
        }

        private bool stopQuiz = false;

        public ICommand StopCommand { get; }
        private void ExecuteEnd(object parameter)
        {
            if (MessageBox.Show("Czy na pewno chcesz wyjść?", "Wyjść?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                stopQuiz = true;
                if (bWorker.IsBusy)
                {
                    bWorker.CancelAsync();
                }
                Application.Current.MainWindow.DataContext = new StartViewModel();
            }
        }

        private bool _lblWrongBool;
        public bool lblWrongBool
        {
            get { return _lblWrongBool; }
            set
            {
                _lblWrongBool = value;
                OnPropertyChanged("lblWrongBool");
            }
        }

        private bool _lblRightBool;
        public bool lblRightBool
        {
            get { return _lblRightBool; }
            set
            {
                _lblRightBool = value;
                OnPropertyChanged("lblRightBool");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
