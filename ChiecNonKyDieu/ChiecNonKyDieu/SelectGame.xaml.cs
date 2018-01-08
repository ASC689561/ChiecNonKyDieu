using ChiecNonKyDieu.Component;
using ChiecNonKyDieu.database;
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
            this.Loaded += SelectGame_Loaded;
        }

        private void SelectGame_Loaded(object sender, RoutedEventArgs e)
        {
            txtUser.Focus();
        }

        public List<Tuple<string, string>> numberPlayerList = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("1 Nhóm chơi","1" ),
            new Tuple<string, string>("2 Nhóm chơi","2"),
            new Tuple<string, string>("3 Nhóm chơi","3"),
            new Tuple<string, string>("4 Nhóm chơi","4"),

        };
        public List<Tuple<string, string>> classList = new List<Tuple<string, string>>()
        {
           new Tuple<string, string>(  "Khối 1","Khoi1"),
            new Tuple<string, string>( "Khối 2","Khoi2"),
            new Tuple<string, string>( "Khối 3","Khoi3"),
            new Tuple<string, string>( "Khối 4","Khoi4"),
            new Tuple<string, string>( "Khối 5","Khoi5")
        };

        int cbbNumberPlayerIndex { get; set; } = 0;
        int classIndex { get; set; } = 0;

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            if (!UserManager.CheckValidUser(txtUser.Text, txtPass.Password))
            {
                CustomForms.MessageBoxForm.Show("", "Tên người dùng hoặc mật khẩu không đúng !", MessageBoxButton.OK, CustomForms.MessageBoxImage.Warning);
                txtUser.Focus();
                return;
            }

            
            StaticData.Khoi = classList[classIndex % classList.Count].Item2;
            StaticData.SoNguoiChoi = int.Parse(numberPlayerList[cbbNumberPlayerIndex % numberPlayerList.Count].Item2);
            this.Visibility = Visibility.Hidden;
            Done?.Invoke(null, null);
        }

        private void NumberPlayerClick(object sender, RoutedEventArgs e)
        {
            cbbNumberPlayerIndex++;
            btnNumberPlayer.Content = numberPlayerList[cbbNumberPlayerIndex % numberPlayerList.Count].Item1;
        }

        private void ClassClick(object sender, RoutedEventArgs e)
        {
            classIndex++;
            btnClass.Content = classList[classIndex % classList.Count].Item1;
        }
    }
}
