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

            public Hit Touche { get; set; }//yo mama so fat she got two watches, one for each time zone she's in.
        }
    }
}
