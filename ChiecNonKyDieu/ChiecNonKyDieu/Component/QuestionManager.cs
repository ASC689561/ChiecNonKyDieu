using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ChiecNonKyDieu.Component
{
    public interface IQuestionManager
    {
        bool QuestionAnwser();
    }


    public class QuestionManager : IQuestionManager
    {
        public bool QuestionAnwser()
        {
            return QuestionAnswer.Instance.Show();

            return MessageBox.Show("Tra loi dung ?", "Tra loi", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
