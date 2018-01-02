using ChiecNonKyDieu.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for PlayerPanle.xaml
    /// </summary>
    public partial class PlayerPanel : UserControl
    {
        public PlayerPanel()
        {
            InitializeComponent();
        }
        Player player;
        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                if (value != null)
                {
                    player = value;
                    DataContext = Player;
                    player.PropertyChanged += Player_PropertyChanged;
                    UpdateSelected();
                }
                else
                {
                    player = null;
                    DataContext = null;
                }
            }
        }

        private void Player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsActive")
                UpdateSelected();
        }
        void UpdateSelected()
        {
            var url = "";
            if (Player != null && Player.IsActive)
                url = "pack://application:,,,/Resources/Player-Active.png";
            else
                url = "pack://application:,,,/Resources/Player.png";

            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri(url, UriKind.Absolute));
            grid.Background = myBrush;
        }


    }
}
