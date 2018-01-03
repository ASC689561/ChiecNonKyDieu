using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ChiecNonKyDieu.Component
{
    static class Utils
    {
        public static Random Random = new Random();
        public static void SetRtf(this RichTextBox rtb, string document)
        {
            var documentBytes = Encoding.UTF8.GetBytes(document);
            using (var reader = new MemoryStream(documentBytes))
            {
                reader.Position = 0;
                rtb.SelectAll();
                rtb.Selection.Load(reader, DataFormats.Rtf);
            }
        }

        public static string GetDataPath()
        {
            var startupPath = System.IO.Path.GetFullPath(System.Windows.Forms.Application.StartupPath);
            return System.IO.Path.Combine(startupPath, "Data");
        }

        public static void Sleep(int timeInSec)
        {
            var time = DateTime.Now.AddSeconds(timeInSec);
            while (DateTime.Now < time)
                System.Windows.Forms.Application.DoEvents();
        }
    }
}
