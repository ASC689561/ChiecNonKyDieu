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

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for TypeSelection.xaml
    /// </summary>
    public partial class TypeSelection : UserControl
    {
        private ManualResetEvent mre = new ManualResetEvent(false);
        public double MaxOpac = 0.9;

        public static TypeSelection Instance;

        public TypeSelection()
        {
            InitializeComponent();
            Instance = this;
        }


        public void Show()
        {
            this.Visibility = Visibility.Visible;
            Facein();
            mre.Reset();

            while (!mre.WaitOne(0))
                System.Windows.Forms.Application.DoEvents();

            mre.WaitOne();
            Faceout();
            Visibility = Visibility.Collapsed;
        }
        private void Faceout()
        {
            while (Opacity > 0)
            {
                Opacity -= 0.01;
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

        private void TypeButton_Clicked(object sender, EventArgs e)
        {
            mre.Set();
        }
    }
}
