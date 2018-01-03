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

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            StaticData.Khoi = (cbbKhoi.SelectedItem as ComboBoxItem).Tag.ToString();
            StaticData.SoNguoiChoi = int.Parse((cbbSoNguoiChoi.SelectedItem as ComboBoxItem).Tag.ToString());
            this.Visibility = Visibility.Hidden;
            Done?.Invoke(null, null);
        }
    }
}
