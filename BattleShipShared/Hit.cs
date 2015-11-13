using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipShared
{
    namespace Packet
    {
        [Serializable]
        public class Hit
        {
            /// <summary>
            /// États du Hit
            /// </summary>
            public enum HitState
            {
                Hit,
                Flop,
                NoAction,
                CoulerPorteAvion,
                CoulerCroiseur,
                CoulerContreTorpilleur,
                CoulerSousMarin,
                CoulerTorpilleur

            }

            /// <summary>
            /// État
            /// </summary>
            public HitState Etat { get; set; }

            /// <summary>
            /// Position du tir
            /// </summary>
            public Point Location { get; set; }
        }
    }
}
