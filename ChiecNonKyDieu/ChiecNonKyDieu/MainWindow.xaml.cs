using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Storyboard sb = (Storyboard)this.vongQuay.FindResource("spin");
                sb.Begin();
                sb.SetSpeedRatio(27);//sample data
            }
            catch
            {
            }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(MainWindow), new UIPropertyMetadata(0.0, new PropertyChangedCallback(AngleChanged)));

        private static void AngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainWindow control = (MainWindow)sender;
            control.PerformAnimation((double)e.OldValue, (double)e.NewValue);
        }

        private void PerformAnimation(double oldValue, double newValue)
        {
            Storyboard s = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = oldValue;
            animation.To = newValue;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            s.Children.Add(animation);


            Storyboard.SetTarget(animation, vongQuay);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Ellipse.RenderTransform).(RotateTransform.Angle)"));

            s.Begin();

        }

        private void vongQuay_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
