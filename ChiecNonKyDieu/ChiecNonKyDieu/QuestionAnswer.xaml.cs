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

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for QuestionAnswer.xaml
    /// </summary>
    public partial class QuestionAnswer : UserControl
    {
        const double MaxOpac = 0.95;
        ManualResetEvent mre = new ManualResetEvent(false);

        public string Answer { get; set; }
        public static QuestionAnswer Instance { get; set; }

        public QuestionAnswer()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            Answer = btn.Content.ToString();
            mre.Set();
        }

        public bool Show(string rtf, string goal)
        {
            richtext.SetRtf(rtf);
            this.Visibility = Visibility.Visible;
            Facein();
            mre.Reset();

            while (!mre.WaitOne(0))
                System.Windows.Forms.Application.DoEvents();

            mre.WaitOne();
            Faceout();
            this.Visibility = Visibility.Collapsed;
            return Answer.ToLower().Trim() == goal.ToLower().Trim();
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
