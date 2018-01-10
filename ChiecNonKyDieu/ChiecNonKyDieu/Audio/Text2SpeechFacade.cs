using ChiecNonKyDieu.Component;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChiecNonKyDieu.Audio
{
    /// <summary>
    /// Text 2 speech
    /// </summary>
    public class Text2SpeechFacade
    {
        static IText2Speech respon = new GoogleVoiceStream();
        // List các cancel token sử dụng để stop khi đang nói
        static ConcurrentBag<CancellationTokenSource> cancelTokenSource = new ConcurrentBag<CancellationTokenSource>();
        private static int playCount;

        private static int PlayCount
        {
            get { return playCount; }
            set
            {
                playCount = value;
                Console.WriteLine(playCount);
            }
        }

        /// <summary>
        /// kiểm tra xem có đang nói hay không
        /// </summary>
        public static bool IsPlaying
        {
            get { return PlayCount != 0; }
        }

        public static Task PlayFile(string file)
        {
            StopAll();
            return Task.Factory.StartNew(() =>
            {
                var cancelToken = new CancellationTokenSource();
                cancelTokenSource.Add(cancelToken);

                var ctk = new CancellationTokenSource();
                cancelTokenSource.Add(ctk);
                respon.PlayFile(file, ctk);
            });
        }


        public static Task PlayWithCache(string texts, int delayTime = 0, string lang = "vi", string cacheName = "")
        {
            string path = "Cache";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var filePath = Path.Combine(path, Utils.CalculateMD5Hash(texts) + "_" + cacheName + ".mp3");
            var hashFile = File.Exists(filePath);
            if (!hashFile)
            {
                var s = (respon as GoogleVoiceStream).GetStream(texts, lang);
                using (var fileStream = File.Create(filePath))
                {
                    s.CopyTo(fileStream);
                }
            }
            return PlayFile(filePath);
        }

        public static Task Play(string texts, int delayTime = 0, string lang = "vi")
        {
            StopAll();
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    PlayCount++;
                    Console.WriteLine("Speak texts:" + texts);
                    // cancel token của vòng for
                    var cancelToken = new CancellationTokenSource();
                    cancelTokenSource.Add(cancelToken);

                    var lines = texts.Split(new[] { Environment.NewLine, "\\r\\n", "\\n\\r", "\\r", "\\n" }, StringSplitOptions.RemoveEmptyEntries);
                    lines = lines.Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
                    foreach (var text in lines)
                    {
                        if (cancelToken.IsCancellationRequested)
                            break;
                        // cancel token của thread text2speech
                        var ctk = new CancellationTokenSource();
                        // Thêm vào list để quản lý và stop khi cần
                        cancelTokenSource.Add(ctk);
                        // Speech
                        respon.Play(text, ctk, lang);
                        Thread.Sleep(delayTime);
                    }
                }
                finally
                {
                    PlayCount--;
                }

            });
        }

        public static void Init()
        {

        }

        public static void StopAll()
        {
            CancellationTokenSource result;
            // cancel tất cả các token source đang có và remove khỏi list
            while (cancelTokenSource.TryTake(out result))
                result.Cancel();
        }
    }
}
