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
        public class PosShips
        {
            public enum Orientation
            {
                Verticale,
                Horizontale
            }
            public Point PPorteAvion;
            public Orientation OPorteAvion;

            public Point PCroiseur;
            public Orientation OCroiseur;

            public Point PContreTorpilleur;
            public Orientation OContreTorpilleur;

            public Point PSousMarin;
            public Orientation OSousMarin;

            public Point PTorpilleur;
            public Orientation OTorpilleur;



        }
    }
}
