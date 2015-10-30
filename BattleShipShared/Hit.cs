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
            public enum HitState
            {
                Hit,
                CoulerPorteAvion,
                CoulerCroiseur,
                CoulerContreTorpilleur,
                CoulerSousMarin,
                CoulerTorpilleur

            }

            public HitState Etat { get; set; }

            Point Location { get; set; }
        }
    }
}
