using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ChiecNonKyDieu.Component
{
    public interface IQuestionManager
    {
        bool QuestionAnwser();
    }

    class NoQuestionException : Exception
    {

    }
    public class Question
    {
        public string Name { get; set; }
        public string Rtf { get; set; }
        public string Goal { get; set; }
        public string Lang { get; set; }

    }

    public class QuestionManager : IQuestionManager
    {



        public bool QuestionAnwser()
        {
            Utils.Sleep(2);
        loop:
            try
            {
                var type = TypeSelection.Instance.Show();
                var question = SelectQuestion(StaticData.Khoi, type);
                return QuestionAnswer.Instance.Show(question, type);
            }
            catch (NoQuestionException ex)
            {
                CustomForms.MessageBoxForm.Show("", "Chưa có câu hỏi nào phù hợp với người chơi, Xin mời chọn lại!", MessageBoxButton.OK, CustomForms.MessageBoxImage.Information);
                goto loop;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Question SelectQuestion(string khoi, string type)
        {
            type = type.ToLower();
            khoi = khoi.ToLower();
            if (!Dic.ContainsKey(khoi) || !Dic[khoi].ContainsKey(type))
                throw new NoQuestionException();
            var ind = Utils.Random.Next(Dic[khoi][type].Count);
            return new Question
            {
                Rtf = File.ReadAllText(Dic[khoi][type][ind].FileName),
                Goal = Dic[khoi][type][ind].Goal,
                Name = Path.GetFileNameWithoutExtension(Dic[khoi][type][ind].FileName),
                Lang = Dic[khoi][type][ind].Lang
            };
        }

        public class FileNameStructure
        {
            static Dictionary<string, string> langDic = new Dictionary<string, string>()
            {
                ["math"] = "en",
                ["english"] = "en",
                ["science"] = "en",
                ["toan"] = "vi",
                ["tiengviet"] = "vi",
                ["khoahoc"] = "vi",
            };

            public string FileName { get; set; }
            public string Khoi { get; set; }
            public string Goal { get; set; }
            public string Loai { get; set; }
            public string Lang { get; set; }

            public static FileNameStructure Parse(string fileName)
            {
                var arr = Path.GetFileNameWithoutExtension(fileName).Split('-');
                return new FileNameStructure
                {
                    FileName = fileName,
                    Goal = arr[2].ToLower(),
                    Khoi = arr[0].ToLower(),
                    Loai = arr[1].ToLower(),
                    Lang = langDic[arr[1].ToLower()]
                };
            }
        }

        public static Dictionary<string, Dictionary<string, List<FileNameStructure>>> Dic { get; set; }

        public static void InitDic(IProgress progressor)
        {
            Dic = new Dictionary<string, Dictionary<string, List<FileNameStructure>>>();
            var files = Directory.GetFiles(Utils.GetDataPath());
            int i = 0;
            foreach (var item in files)
            {
                i++;
                progressor.SetProgress(i * 1.0 / files.Length);
                try
                {
                    var file = FileNameStructure.Parse(item);
                    if (!Dic.ContainsKey(file.Khoi))
                        Dic.Add(file.Khoi, new Dictionary<string, List<FileNameStructure>>());
                    if (!Dic[file.Khoi].ContainsKey(file.Loai))
                        Dic[file.Khoi].Add(file.Loai, new List<FileNameStructure>());
                    Dic[file.Khoi][file.Loai].Add(file);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Đọc file lỗi:" + item);
                }
            }
        }
    }
}
