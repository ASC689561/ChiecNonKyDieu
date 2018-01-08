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

        public void SetPlayer(PlayerManager mgr)
        {
            for (int i = 0; i < mgr.Players.Count; i++)
            {
                mgr.Players[i].Name = "Player " + (i + 1);
            }

            player1.Player = null;
            player2.Player = null;
            player3.Player = null;
            player4.Player = null;

            player1.Visibility = Visibility.Hidden;
            player2.Visibility = Visibility.Hidden;
            player3.Visibility = Visibility.Hidden;
            player4.Visibility = Visibility.Hidden;

            if (mgr.Players.Count > 0)
            {
                player1.Player = mgr.Players[0];
                player1.Visibility = Visibility.Visible;
            }

            if (mgr.Players.Count > 1)
            {
                player2.Player = mgr.Players[1];
                player2.Visibility = Visibility.Visible;
            }
            if (mgr.Players.Count > 2)
            {
                player3.Player = mgr.Players[2];
                player3.Visibility = Visibility.Visible;
            }
            if (mgr.Players.Count > 3)
            {
                player4.Player = mgr.Players[3];
                player4.Visibility = Visibility.Visible;
            }
        }

        bool mouseHold = false;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

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

            if (lockCount > 0)
                return;

            lockCount = 1;
            vongQuay.Start(1 - progressbar.Value * 1.0 / 100);
        }

        private void cvVongQuay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lockCount > 0)
                return;

            mouseHold = true;
            StartNewThread();
        }

        public int lockCount = 0;


        private void InitGame(object sender, EventArgs e)
        {
            vongQuay = new VongQuay(this.TransRotate, TransRotate_MuiTen);
            PlayerManager = new PlayerManager(StaticData.SoNguoiChoi);
            SetPlayer(PlayerManager);

            vongQuay.Stopped += (o1, e1) =>
            {
                try
                {
                    PlayerManager.ProcessRollingValue(e1.CurrentValue);
                }
                catch (Exception ex)
                {

                }

                this.lockCount = 0;
            };
            vongQuay.ValueChanged += (o, e1) =>
            {
                label.Content = e1.NewValue;
            };

            this.DataContext = PlayerManager;
        }
    }
}
