using ChiecNonKyDieu.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        IVongQuay vongQuay;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vongQuay = new VongQuay(this.TransRotate);
            //ComponentDispatcher.ThreadIdle += new System.EventHandler(ComponentDispatcher_ThreadIdle);
            //Application.id
        }

        //int v = 360;
        //private void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        //{
        //    if (v <= 0)
        //        return;

        //    TransRotate.Angle = v--;
        //}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            vongQuay.Start(100);
        }

        private void cvVongQuay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            vongQuay.Start(100);

        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            TransRotate.Angle += 1;
            Console.WriteLine(TransRotate);
        }
    }
}
