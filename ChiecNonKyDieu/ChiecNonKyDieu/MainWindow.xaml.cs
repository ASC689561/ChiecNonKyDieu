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
            vongQuay = new VongQuay(this.TransRotate, TransRotate_MuiTen);
        }
        static Random r = new Random();

        private void cvVongQuay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            vongQuay.Start(r.NextDouble());

        }

    }
}
