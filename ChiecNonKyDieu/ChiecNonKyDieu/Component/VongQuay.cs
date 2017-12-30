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
        double currentValue = 0;
        double step = 2;
        private RotateTransform transRotate;
        private List<string> score = new List<string>();
        public VongQuay(RotateTransform transRotate)
        {
            this.transRotate = transRotate;

            score.AddRange(new[]
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
            });
        }

        private void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            transRotate.Angle = GetCurrentValue();
        }

        double padding = 10;

        private double GetCurrentValue()
        {
            Console.WriteLine(currentValue);
            currentValue -= step;
            step -= delta;
            if (step <= 0)
                Stop();
            return currentValue;
        }

        static Random rnd = new Random();
        double delta = 0.001f;
        public void Start(double value)
        {
            delta = rnd.NextDouble() * (0.0012f - 0.0005f) + 0.0005f;

            step = 2;
            currentValue = 0;
            ComponentDispatcher.ThreadIdle += new EventHandler(ComponentDispatcher_ThreadIdle);
        }

        public void Stop()
        {
            ComponentDispatcher.ThreadIdle -= new EventHandler(ComponentDispatcher_ThreadIdle);
            var rate = 360 / score.Count;
            var frac = Math.Abs((currentValue-7) % 360);
            var index = frac / rate;
            Console.WriteLine(score[((int)index) % score.Count]);
        }


    }
}
