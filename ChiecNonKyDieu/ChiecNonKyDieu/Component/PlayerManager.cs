using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiecNonKyDieu.Component
{
    public class Player : NotifyBase
    {
        bool isActive;
        int currentScore;
        string name;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                base.OnPropertyChanged("IsActive");
            }
        }

        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
            set
            {
                currentScore = value;
                base.OnPropertyChanged("CurrentScore");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                base.OnPropertyChanged("Name");
            }
        }

        public Player()
        {
            Name = Environment.TickCount.ToString();
        }
    }

    class NumberPlayerInvalidException : Exception
    {

    }

    public class PlayerManager : IPlayerManager
    {
        public IQuestionManager QuestionManager { get; set; } = new QuestionManager();
        Player activePlayer;
        public IList<Player> Players { get; set; } = new List<Player>();
        public Player ActivePlayer
        {
            get { return activePlayer; }
            set
            {
                for (int i = 0; i < Players.Count; i++)
                    Players[i].IsActive = false;
                activePlayer = value;
                activePlayer.IsActive = true;
            }
        }


        public PlayerManager(int number)
        {
            if (number < 0 || number > 4)
                throw new NumberPlayerInvalidException();

            for (int i = 0; i < number; i++)
                Players.Add(new Player());
            this.ActivePlayer = Players.First();
        }

        public void ProcessRollingValue(RollingValueBase rollValue)
        {
            rollValue.Do(this);
        }

        public void NextPlayer()
        {
            var index = Players.IndexOf(activePlayer);
            index = index + 1;
            index = index % Players.Count;
            ActivePlayer = Players[index];
        }
    }

}
