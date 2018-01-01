using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ChiecNonKyDieu.Component
{
    /// <summary>
    /// Giao diện quản lý người chơi và lượt chơi
    /// </summary>
    public interface IPlayerManager
    {
        /// <summary>
        /// Người chơi hiện tại
        /// </summary>
        Player ActivePlayer { get; set; }


        /// <summary>
        /// Danh sách những người chơi đang có
        /// </summary>
        IList<Player> Players { get; set; }


        /// <summary>
        /// Xử lý 1 kết quả vòng quay
        /// </summary>
        /// <param name="rollValue"></param>
        void ProcessRollingValue(RollingValueBase rollValue);

        IQuestionManager QuestionManager { get; set; }

        void NextPlayer();
    }
}
