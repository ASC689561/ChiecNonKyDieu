using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiecNonKyDieu.Component
{

    public abstract class RollingValueBase
    {
        public virtual void Do(IPlayerManager playerManager)
        {

        }
    }

    public class Scorevalue : RollingValueBase
    {
        public int Score { get; set; }

        public Scorevalue(int score)
        {
            this.Score = score;
        }

        public override void Do(IPlayerManager playerManager)
        {
            var result = playerManager.QuestionManager.QuestionAnwser();
            if (result) // Nếu trả lời đúng 
            {
                playerManager.ActivePlayer.CurrentScore += Score;
            }
            else // Nếu trả lời sai 
            {
                playerManager.NextPlayer();
            }
        }

        public override string ToString()
        {
            return Score.ToString();
        }
    }

    public class ChiaDoi : RollingValueBase
    {
        public override void Do(IPlayerManager playerManager)
        {
            var result = playerManager.QuestionManager.QuestionAnwser();
            playerManager.ActivePlayer.CurrentScore /= 2;

            if (result) // Nếu trả lời đúng  thì không được công điểm và được quay tiếp
            {

            }
            else // Nếu trả lời sai thì mất lượt
            {
                playerManager.NextPlayer();
            }

        }
        public override string ToString()
        {
            return "Chia đôi";
        }
    }

    public class ThemLuot : RollingValueBase
    {
        public override void Do(IPlayerManager playerManager)
        {
            throw new Exception("Khong xu ly"); 
        }
        public override string ToString()
        {
            return "Thêm lượt";
        }
    }

    public class MatDiem : RollingValueBase
    {
        public override void Do(IPlayerManager playerManager)
        {
            var result = playerManager.QuestionManager.QuestionAnwser();
            playerManager.ActivePlayer.CurrentScore = 0; ;

            if (result) // Nếu trả lời đúng  thì không được công điểm và được quay tiếp
            {

            }
            else // Nếu trả lời sai thì mất lượt
            {
                playerManager.NextPlayer();
            }

        }
        public override string ToString()
        {
            return "Mất điểm";
        }
    }
    public class MayMan : RollingValueBase
    {
        public override void Do(IPlayerManager playerManager)
        {
            throw new Exception("Khong xu ly"); 
        }
        public override string ToString()
        {
            return "May mắn";
        }
    }
    public class MatLuot : RollingValueBase
    {
        public override void Do(IPlayerManager playerManager)
        {
            playerManager.NextPlayer();
        }

        public override string ToString()
        {
            return "Mất lượt";
        }
    }
    public class NhanDoi : RollingValueBase
    {
        public override void Do(IPlayerManager playerManager)
        {
            var result = playerManager.QuestionManager.QuestionAnwser();

            if (result) // Nếu trả lời đúng nhân thì đôi điểm 
            {
                playerManager.ActivePlayer.CurrentScore *= 2;

            }
            else // Nếu trả lời sai thì mất lượt
            {
                playerManager.NextPlayer();
            }
        }


        public override string ToString()
        {
            return "Nhân đôi";
        }
    }
}
