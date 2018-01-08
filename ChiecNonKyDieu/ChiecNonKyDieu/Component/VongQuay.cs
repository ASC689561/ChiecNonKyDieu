using ChiecNonKyDieu.Audio;
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
using System.Windows.Threading;

namespace ChiecNonKyDieu.Component
{
    class FrameMarker
    {
        int currentValue;

        public FrameMarker(int n)
        {
            this.N = n;
        }

        public void Mark()
        {
            currentValue = N;
        }

        public bool IsMarked
        {
            get
            {
                try
                {
                    if (currentValue > 0)
                        return true;
                    return false;
                }
                finally
                {
                    currentValue--;
                }
            }
        }

        public int N { get; private set; }

        internal void Reset()
        {
            currentValue = 0;
        }
    }

    class VongQuay : IVongQuay
    {
        static Random rnd = new Random();
        public event EventHandler<IndexChangedEventArgs<RollingValueBase>> ValueChanged;
        public event EventHandler<RollingCompletedEventArgs> Stopped;

        const double padding = 10;

        public double CurrentValue { get; set; }

        RotateTransform transRotate;
        RotateTransform transRotate_MuiTen;

        Decrease decrease = new Decrease(5);
        FrameMarker maker = new FrameMarker(3);
        ChangeMonitor<int> IndexMonitor = new ChangeMonitor<int>();
        DispatcherTimer timer;

        List<RollingValueBase> score = new List<RollingValueBase>()
        {
                new Scorevalue( 2000),
                new Scorevalue(300),
                new ChiaDoi(),
                new Scorevalue(700),
                new Scorevalue(900),
                new Scorevalue(200),
                new Scorevalue(600),
                new Scorevalue(300),
                new Scorevalue(600),
                new Scorevalue(400),
                new Scorevalue(1000),
                new MatDiem(),
                new Scorevalue(800),
                new Scorevalue(300),
                new Scorevalue(200),
                new Scorevalue(900),
                new MatLuot(),
                new Scorevalue(400),
                new Scorevalue(1000),
                new Scorevalue(700),
                new Scorevalue(200),
                new NhanDoi(),
                new Scorevalue(500),
                new Scorevalue(100)
        };



        public VongQuay(RotateTransform transRotate, RotateTransform transRotate_MuiTen)
        {
            this.transRotate = transRotate;
            this.transRotate_MuiTen = transRotate_MuiTen;
            IndexMonitor.ValueChanged += (o, e) =>
            {
                maker.Mark();
                ValueChanged?.Invoke(this, new IndexChangedEventArgs<RollingValueBase> { OldValue = score[e.OldValue], NewValue = score[e.NewValue] });
            };

            timer = new DispatcherTimer(TimeSpan.FromMilliseconds(1),
                DispatcherPriority.Background,
                ComponentDispatcher_ThreadIdle,
                System.Windows.Application.Current.Dispatcher
            );
            timer.Stop();
        }


        public void Start(double value)
        {
            Text2SpeechFacade.PlayFile("resources/nhac_quay.mp3");

            if (value <= 0)
                value = 0.05;
            if (value >= 1)
                value = 0.95;

            decrease.Reset(value);
            timer.Start();
        }

        public void Stop()
        {
            Text2SpeechFacade.StopAll();

            timer.Stop();
            Stopped?.Invoke(this, new RollingCompletedEventArgs { CurrentValue = score[CurrentValueIndex] });
            maker.Reset();
            transRotate_MuiTen.Angle = 0;
        }

        public int CurrentValueIndex
        {
            get
            {
                var rate = 360 / score.Count;
                var frac = Math.Abs((CurrentValue - 7) % 360);
                var index = frac / rate;
                return ((int)index) % score.Count;
            }
        }

        #region MyRegion

        double GetCurrentAngle()
        {
            var step = decrease.Step;
            CurrentValue -= step;
            if (step <= 0)
                Stop();
            return CurrentValue;
        }


        DateTime lastedTime = DateTime.Now;

        private void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastedTime).TotalMilliseconds <= 1)
                return;
            
            transRotate.Angle = GetCurrentAngle();
            IndexMonitor.NotifyNewValue(CurrentValueIndex);
            transRotate_MuiTen.Angle = maker.IsMarked ? 10 : 0;
            lastedTime = DateTime.Now;
        }

        #endregion

    }

    class Decrease
    {
        private const float MinValue = 0.0009f;
        private const float MaxValue = 0.01f;
        private double delta;
        private double step;
        private double stepOriginal;

        public Decrease(double step = 2)
        {
            this.stepOriginal = step;
        }

        public void Reset(double value)
        {
            delta = value * (MaxValue - MinValue) + MinValue;
            step = stepOriginal;
        }

        public double Step
        {
            get
            {
                step -= delta;
                return step;
            }
        }

    }

    public class ChangeMonitor<T>
    {
        public T OldValue { get; set; }
        public event EventHandler<IndexChangedEventArgs<T>> ValueChanged;

        public void NotifyNewValue(T newValue)
        {
            if (!OldValue.Equals(newValue))
            {
                ValueChanged?.Invoke(this, new IndexChangedEventArgs<T>() { OldValue = OldValue, NewValue = newValue });
                OldValue = newValue;
            }
        }
    }

    public class RollingCompletedEventArgs : EventArgs
    {
        public RollingValueBase CurrentValue { get; set; }
    }
    public class IndexChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; set; }
        public T NewValue { get; set; }
    }
}
