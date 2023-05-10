using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCARDS.Model
{
    public class StateMachine
    {
        public enum GameState : int
        {
            Title_Screen,
            Main_Menu,
            Level_1,
            Level_2,
            Cutscene
        }

        public static GameState gameState;
    }
}
