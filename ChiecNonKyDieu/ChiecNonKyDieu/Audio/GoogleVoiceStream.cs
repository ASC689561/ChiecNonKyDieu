using ChiecNonKyDieu.Properties;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace ChiecNonKyDieu.Audio
{

    public class GoogleVoiceStream : IText2Speech
    {
        private object syn = new object();

        private static string GetUrl(string text, string lang = "vi")
        {
            text = text.Replace("?", "");
            text = text.Replace("...", "ba chấm");
            text = System.Web.HttpUtility.UrlEncode(text);
            return string.Format(Settings.Default.SpeechUrl, text, lang);
        }

        public void Play(string text, CancellationTokenSource ctk, string lang = "vi")
        {
            try
            {
                lock (syn)
                      PlayMp3FromUrl(GetUrl(text, lang), ctk, 30000);
            }
            catch (Exception)
            {

            }

        }

        public MemoryStream GetStream(string text, string lang = "vi")
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(GetUrl(text, lang));

            return new MemoryStream(imageData);

        }
        public void PlayFile(string filePath, CancellationTokenSource ctk)
        {
            try
            {
                var stop = new AutoResetEvent(false);

                using (Stream ms = new MemoryStream())
                {
                    using (Stream stream = File.OpenRead(filePath))
                    {
                        byte[] buffer = new byte[32768];
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            ms.Write(buffer, 0, read);
                    }

                    ms.Position = 0;
                    using (var blockAlignedStream = new CustomWaveProvider(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms)), stop, ctk))
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.PlaybackStopped += (sender, e) =>
                        {
                            waveOut.Stop();
                            stop.Set();
                        };
                        waveOut.Play();
                        stop.WaitOne(30000);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void PlayMp3FromUrl(string url, CancellationTokenSource ctk, int timeout = 3000)
        {
            var stop = new AutoResetEvent(false);

            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url).GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        ms.Write(buffer, 0, read);
                }

                ms.Position = 0;
                using (var blockAlignedStream = new CustomWaveProvider(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms)), stop, ctk))
                using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                {
                    waveOut.Init(blockAlignedStream);
                    waveOut.PlaybackStopped += (sender, e) =>
                    {
                        waveOut.Stop();
                        stop.Set();
                    };
                    waveOut.Play();
                    stop.WaitOne(timeout);
                }
            }
        }

        class CustomWaveProvider : IWaveProvider, IDisposable
        {
            public WaveFormat WaveFormat
            {
                get { return waveStream.WaveFormat; }
            }

            private int silencePos = 0;
            private int silenceBytes = 8000;
            private WaveStream waveStream;
            private bool end;
            private AutoResetEvent mre;
            private CancellationTokenSource tokenSource;

            public CustomWaveProvider(WaveStream waveStream, AutoResetEvent mre, CancellationTokenSource tokenSrc)
            {
                this.waveStream = waveStream;
                this.mre = mre;
                tokenSource = tokenSrc;
            }

            public int Read(byte[] buffer, int offset, int count)
            {
                if (end || tokenSource.IsCancellationRequested)
                {
                    mre.Set();
                    return 0;
                }

                int bytesRead = waveStream.Read(buffer, offset, count);
                if (bytesRead < count)
                {
                    end = true;
                    int silenceToAdd = Math.Min(silenceBytes - silencePos, count - bytesRead);
                    for (var i = offset + bytesRead; i < offset + bytesRead + silenceToAdd; i++)
                        buffer[i] = 0;

                    bytesRead += silenceToAdd;
                    silencePos += silenceToAdd;
                }
                return bytesRead;
            }

            public void Dispose()
            {
                waveStream.Dispose();
            }
        }
    }
}
