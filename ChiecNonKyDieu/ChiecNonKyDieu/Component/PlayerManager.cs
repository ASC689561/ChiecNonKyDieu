using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiecNonKyDieu.Component
{
    public class Player
    {
        public int CurrentScore { get; set; }
        public string Name { get; set; }

        public Player()
        {
            Name = Environment.TickCount.ToString();
        }
    }

    class NumberPlayerInvalidException : Exception
    {

    }
    public class PlayerManager
    {
        private IVongQuay vongQuay;
        public List<Player> Players { get; set; } = new List<Player>();
        public Player ActivePlayer { get; set; }

        public PlayerManager(IVongQuay vongQuay, int number)
        {
            if (number < 0 || number > 4)
                throw new NumberPlayerInvalidException();

            this.vongQuay = vongQuay;
            for (int i = 0; i < number; i++)
                Players.Add(new Player());
            this.ActivePlayer = Players.First();
            vongQuay.Stopped += VongQuay_Stopped;
        }

        private void VongQuay_Stopped(object sender, RollingCompletedEventArgs e)
        {
            if (e.CurrentValue is Scorevalue)
                ActivePlayer.CurrentScore += (e.CurrentValue as Scorevalue).Score;
        }


        public void Roll(double speed)
        {
            vongQuay.Start(speed);
        }
    }

    public abstract class RollingValueBase
    {
        public virtual void Do(PlayerManager playerManager)
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
    }

    public class ChiaDoi : RollingValueBase
    {

    }

    public class ThemLuot : RollingValueBase
    {

    }

    public class MatDiem : RollingValueBase
    {

    }
    public class MayMan : RollingValueBase
    {

    }
    public class MatLuot : RollingValueBase
    {

    }
    public class NhanDoi : RollingValueBase
    {

    }
}
