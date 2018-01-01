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
        static Random r = new Random();
        IVongQuay vongQuay;
        public PlayerManager PlayerManager { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        bool mouseHold = false;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vongQuay = new VongQuay(this.TransRotate, TransRotate_MuiTen);
            PlayerManager = new PlayerManager(2);
            vongQuay.Stopped += (o1, e1) =>
            {
                PlayerManager.ProcessRollingValue(e1.CurrentValue);
            };
            vongQuay.ValueChanged += (o, e1) =>
            {
                label.Content = e1.NewValue;
            };

            this.DataContext = PlayerManager;
        }

        void StartNewThread()
        {
            new Thread(() =>
              {
                  Dispatcher.Invoke(new Action(() =>
                  {
                      progressbar.Value = 0;
                      while (mouseHold)
                      {
                          var value = progressbar.Value + 10;
                          if (value > 110)
                              value = 0;

                          progressbar.Value = value;
                          System.Windows.Forms.Application.DoEvents();
                          Thread.Sleep(200);
                      }
                  }));
              }).Start();
        }

        private void cvVongQuay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseHold = false;
            vongQuay.Start(1 - progressbar.Value * 1.0 / 100);
        }

        private void cvVongQuay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseHold = true;
            StartNewThread();
        }
    }
}
