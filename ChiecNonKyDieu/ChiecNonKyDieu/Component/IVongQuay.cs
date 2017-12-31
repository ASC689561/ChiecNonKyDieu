using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiecNonKyDieu.Component
{
    public interface IVongQuay
    {
        event EventHandler<IndexChangedEventArgs<RollingValueBase>> ValueChanged;
        event EventHandler<RollingCompletedEventArgs> Stopped;
        void Start(double value);
        void Stop();
    }
}
