using ChiecNonKyDieu.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChiecNonKyDieu.Component;
using System.IO;
using ChiecNonKyDieu.Audio;
using System.Windows.Threading;

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for QuestionAnswer.xaml
    /// </summary>
    public partial class QuestionAnswer : UserControl
    {
        DispatcherTimer _timer;
        TimeSpan _time;

        const double MaxOpac = 0.95;
        ManualResetEvent mre = new ManualResetEvent(false);
        private Question question;

        public bool Answer { get; set; }
        public static QuestionAnswer Instance { get; set; }

        public QuestionAnswer()
        {
            InitializeComponent();
            Instance = this;
            _time = TimeSpan.FromSeconds(Settings.Default.Timeout);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                tbTime.Content = _time.TotalSeconds.ToString();
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    Button_Click(null, null);
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);
            _timer.Stop();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            this.IsEnabled = false;
            Text2SpeechFacade.StopAll();
            if ((sender == null))
            {
                Answer = false;
            }
            else
            {
                var btn = sender as Button;
                Answer = btn.Content.ToString().ToLower().Trim() == question.Goal.ToLower().Trim();
            }

            ShowTrueFaild(Answer);
            mre.Set();
        }

        public bool Show(Question question, string type)
        {
            this.question = question;
            IsEnabled = true;
            emoticon_failed.Visibility = Visibility.Hidden;
            emoticon_true.Visibility = Visibility.Hidden;



            richtext.SetRtf(question.Rtf);


            this.Visibility = Visibility.Visible;
            Facein();

            SpeakQuestion(richtext, type);
            _time = TimeSpan.FromSeconds(Settings.Default.Timeout);
            _timer.Start();


            mre.Reset();

            while (!mre.WaitOne(0))
                System.Windows.Forms.Application.DoEvents();

            mre.WaitOne();
            Faceout();
            Visibility = Visibility.Collapsed;

            return Answer;
        }

        private void ShowTrueFaild(bool v)
        {
            if (v)
            {
                Text2SpeechFacade.PlayFile("resources/true.mp3");
                emoticon_true.Visibility = Visibility.Visible;
            }
            else
            {
                Text2SpeechFacade.PlayFile("resources/failed.mp3");
                emoticon_failed.Visibility = Visibility.Visible;
            }

            Utils.Sleep(4);
            emoticon_true.Visibility = Visibility.Hidden;
            emoticon_failed.Visibility = Visibility.Hidden;
        }

        private void SpeakQuestion(RichTextBox richtext, string type)
        {
            string text = new TextRange(richtext.Document.ContentStart, richtext.Document.ContentEnd).Text;
            Text2SpeechFacade.StopAll();
            Text2SpeechFacade.PlayWithCache(text, 0, question.Lang, question.Name);
        }

        private void Faceout()
        {
            while (this.Opacity > 0)
            {
                this.Opacity -= 0.01;
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(Settings.Default.FaceDelay);
            }

        }
        private void Facein()
        {
            while (this.Opacity < MaxOpac)
            {
                this.Opacity += 0.01;
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(Settings.Default.FaceDelay);
            }
        }
    }
}
