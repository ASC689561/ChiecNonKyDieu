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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for SelectGame.xaml
    /// </summary>
    public partial class SelectGame : UserControl
    {
        public event EventHandler<EventArgs> Done;

        public SelectGame()
        {
            InitializeComponent();
        }

        public List<string> numberPlayerList = new List<string>()
        {
            "1 Nhóm chơi",
            "2 Nhóm chơi",
            "3 Nhóm chơi",
            "4 Nhóm chơi",

        };
        public List<string> classList = new List<string>()
        {
            "Khối 1",
            "Khối 2",
            "Khối 3",
            "Khối 4",
            "Khối 5"
        };

        int cbbNumberPlayerIndex { get; set; } = 0;
        int classIndex { get; set; } = 0;

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            //StaticData.Khoi = (cbbKhoi.SelectedItem as ComboBoxItem).Tag.ToString();
            //StaticData.SoNguoiChoi = int.Parse((cbbSoNguoiChoi.SelectedItem as ComboBoxItem).Tag.ToString());
            //this.Visibility = Visibility.Hidden;
            //Done?.Invoke(null, null);
        }

        private void NumberPlayerClick(object sender, RoutedEventArgs e)
        {
            cbbNumberPlayerIndex++;
            btnNumberPlayer.Content = numberPlayerList[cbbNumberPlayerIndex % numberPlayerList.Count];
        }

        private void ClassClick(object sender, RoutedEventArgs e)
        {
            classIndex++;
            btnClass.Content = classList[classIndex % classList.Count];
        }
    }
}
