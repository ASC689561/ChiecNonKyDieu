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

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for QuestionAnswer.xaml
    /// </summary>
    public partial class QuestionAnswer : UserControl
    {
        const double MaxOpac = 0.95;
        ManualResetEvent mre = new ManualResetEvent(false);
        private string goal;

        public string Answer { get; set; }
        public static QuestionAnswer Instance { get; set; }

        public QuestionAnswer()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Text2SpeechFacade.StopAll();

            var btn = sender as Button;
            Answer = btn.Content.ToString();
            ShowTrueFaild(Answer.ToLower().Trim() == goal.ToLower().Trim());
            mre.Set();
        }

        public bool Show(string rtf, string type, string goal)
        {
            this.IsEnabled = true;
            emoticon_failed.Visibility = Visibility.Hidden;
            emoticon_true.Visibility = Visibility.Hidden;

            this.goal = goal;
            richtext.SetRtf(rtf);
            SpeakQuestion(richtext, type);
            this.Visibility = Visibility.Visible;
            Facein();
            mre.Reset();

            while (!mre.WaitOne(0))
                System.Windows.Forms.Application.DoEvents();

            mre.WaitOne();
            Faceout();
            Visibility = Visibility.Collapsed;

            return Answer.ToLower().Trim() == goal.ToLower().Trim();
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
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                ["math"] = "en",
                ["english"] = "en",
                ["science"] = "en",
                ["toan"] = "vi",
                ["tv"] = "vi",
                ["tnxh"] = "vi",
            };

            string text = new TextRange(richtext.Document.ContentStart, richtext.Document.ContentEnd).Text;
            Text2SpeechFacade.StopAll();
            Text2SpeechFacade.Play(text, 0, dic[type.ToLower()]);
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
