using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipShared
{
    namespace Packet
    {
        [Serializable]
        public class Result
        {
            public enum ResultState
            {
                Victory,
                Lose
            }

            public ResultState Etat { get; set; }

            public PosShips EnemyShips { get; set; }
        }
    }
}
