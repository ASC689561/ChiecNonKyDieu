using ChiecNonKyDieu.Audio;
using ChiecNonKyDieu.Component;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChiecNonKyDieu
{
    /// <summary>
    /// Interaction logic for AudioDownloader.xaml
    /// </summary>
    public partial class AudioDownloader : Window
    {
        int selectedIndex;
        public List<QuestionManager.FileNameStructure> Questions { get; set; }

        public AudioDownloader()
        {
            Init();
            InitializeComponent();
            DataContext = this;
            if (Questions.Count > 0)
                SelectedIndex = 0;
        }
         
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (value < 0)
                    return;
                if (value > Questions.Count - 1)
                    return;
                try
                {
                    selectedIndex = value;
                    this.richtext.SetRtf(File.ReadAllText(Questions[selectedIndex].FileName));
                    this.status.Content = "Process file: " + System.IO.Path.GetFileName(Questions[selectedIndex].FileName);
                    var text = new TextRange(richtext.Document.ContentStart, richtext.Document.ContentEnd).Text;
                    Text2SpeechFacade.PlayWithCache(text, 0, Questions[selectedIndex].Lang, System.IO.Path.GetFileNameWithoutExtension(Questions[selectedIndex].FileName));
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Có lỗi khi hiển thị file: {0}", ex.Message));
                }

            }
        }

        private void Init()
        {
            Questions = new List<QuestionManager.FileNameStructure>();

            foreach (var i in QuestionManager.Dic.Values)
                foreach (var t in i.Values)
                    foreach (var q in t)
                        Questions.Add(q);
        }

        private void DownloadAudioClick(object sender, RoutedEventArgs e)
        {
            foreach (var q in Questions)
            {
                try
                {
                    var text = File.ReadAllText(q.FileName);
                    richtext.SetRtf(text);
                    text = new TextRange(richtext.Document.ContentStart, richtext.Document.ContentEnd).Text;
                    Text2SpeechFacade.PlayWithCache(text, 0, q.Lang, System.IO.Path.GetFileNameWithoutExtension(q.FileName));
                    this.status.Content = "Process file: " + System.IO.Path.GetFileName(q.FileName);
                    System.Windows.Forms.Application.DoEvents();
                }
                catch (Exception ex)
                {
                    this.status.Content = "Process file failed" + System.IO.Path.GetFileName(q.FileName);
                }
            }
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"C:\Program Files\Windows NT\Accessories\wordpad.exe", Questions[SelectedIndex].FileName);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Có lỗi khi edit file: " + ex.Message);
            }
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SelectedIndex++;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Có lỗi khi next:" + ex.Message);
            }


        }

        private void PrevClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SelectedIndex--;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Có lỗi khi prev:" + ex.Message);
            }

        }
    }
}
