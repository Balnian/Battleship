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
            /// <summary>
            /// États du Result
            /// </summary>
            public enum ResultState
            {
                Victory,
                Lose
            }

            /// <summary>
            /// État
            /// </summary>
            public ResultState Etat { get; set; }

            /// <summary>
            /// Grille Ennemie
            /// </summary>
            public PosShips EnemyShips { get; set; }

            /// <summary>
            /// Coordonnées du dernier tir de la partie
            /// </summary>
            public Hit Touche { get; set; }
        }
    }
}
