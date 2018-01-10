using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ChiecNonKyDieu.Component
{
    static class Utils
    {
        public static Random Random = new Random();


        public static string CalculateMD5Hash(string input)

        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }

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
