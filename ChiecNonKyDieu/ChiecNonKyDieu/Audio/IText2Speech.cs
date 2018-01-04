using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ChiecNonKyDieu.Audio
{
    public interface IText2Speech
    {
        void PlayFile(string file, CancellationTokenSource ctk);
        void Play(string text, CancellationTokenSource ctk, string lang = "vi");
    }
}
