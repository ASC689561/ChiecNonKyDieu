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

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for QuestionAnswer.xaml
    /// </summary>
    public partial class QuestionAnswer : UserControl
    {
        const double MaxOpac = 0.95;
        ManualResetEvent mre = new ManualResetEvent(false);

        public bool Correct { get; set; }
        public static QuestionAnswer Instance { get; set; }

        public QuestionAnswer()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            Correct = btn.Content.ToString() == "A";
            mre.Set();
        }

        public bool Show()
        {
            this.Visibility = Visibility.Visible;
            Facein();
            mre.Reset();


            while (!mre.WaitOne(0))
                System.Windows.Forms.Application.DoEvents();

            mre.WaitOne();
            Faceout();
            this.Visibility = Visibility.Collapsed;
            return Correct;
        }
        private void Faceout()
        {
            while (this.Opacity > 0)
            {
                this.Opacity -= 0.01;
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);
            }

        }
        private void Facein()
        {
            while (this.Opacity < MaxOpac)
            {
                this.Opacity += 0.01;
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);
            }

        }
    }
}
