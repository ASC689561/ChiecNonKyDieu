using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ChiecNonKyDieu.Component
{
    class VongQuay : IVongQuay
    {
        static Random rnd = new Random();

        private const int MarkCount = 3;
        double step = 2;
        private int Mark;
        const double padding = 10;
        double currentValue;
        double delta = 0.001f;
        RotateTransform transRotate;
        RotateTransform transRotate_MuiTen;

        ChangeMonitor<int> IndexMonitor = new ChangeMonitor<int>();

        List<string> score = new List<string>()
        {
                "2000",
                "300",
                "chia đôi",
                "700",
                "900",
                "200",
                "thêm lượt",
                "300",
                "600",
                "400",
                "1000",
                "mất điểm",
                "800",
                "300",
                "may mắn",
                "600",
                "mất lượt",
                "400",
                "1000",
                "700",
                "200",
                "nhân đôi",
                "500",
                "100"
        };



        public VongQuay(RotateTransform transRotate, RotateTransform transRotate_MuiTen)
        {
            this.transRotate = transRotate;
            this.transRotate_MuiTen = transRotate_MuiTen;
            this.IndexChangedEvent += (o, e) => { Mark = MarkCount; };
        }


        private double GetCurrentValue()
        {
            Console.WriteLine(currentValue);
            currentValue -= step;
            step -= delta;
            if (step <= 0)
                Stop();
            return currentValue;
        }

        public void Start(double value)
        {
            delta = value * (0.0012f - 0.0005f) + 0.0005f;
            step = 2;
            currentValue = 0;
            ComponentDispatcher.ThreadIdle += new EventHandler(ComponentDispatcher_ThreadIdle);
        }

        public void Stop()
        {
            ComponentDispatcher.ThreadIdle -= new EventHandler(ComponentDispatcher_ThreadIdle);
        }

        public int Index
        {
            get
            {
                var rate = 360 / score.Count;
                var frac = Math.Abs((currentValue - padding) % 360);
                var index = frac / rate;
                Console.WriteLine(score[((int)index) % score.Count]);
                return ((int)index) % score.Count;
            }
        }


        public event EventHandler<IndexChangedEventArgs> IndexChangedEvent;

        #region MyRegion

        int oldValue = 0;

        private void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            transRotate.Angle = GetCurrentValue();

            if (oldValue != Index)
            {
                IndexChangedEvent?.Invoke(this, new IndexChangedEventArgs { OldIndex = oldValue, NewIndex = Index });
                oldValue = Index;
            }


            if (Mark > 0)
            {
                transRotate_MuiTen.Angle = 10;
                Mark--;
            }
            else
                transRotate_MuiTen.Angle = 0;
        }

        #endregion

    }

    public class ChangeMonitor<T>
    {
        public T OldValue { get; set; }
        public event EventHandler<IndexChangedEventArgs<T>> ValueChanged;

        public void NotifyNewValue(T newValue)
        {
            if (!OldValue.Equals(newValue))
            {
                ValueChanged?.Invoke(this, new IndexChangedEventArgs<T>() { OldIndex = OldValue, NewIndex = newValue });
                OldValue = newValue;
            }
        }
    }

    public class IndexChangedEventArgs<T> : EventArgs
    {
        public T OldIndex { get; set; }
        public T NewIndex { get; set; }
    }
}
