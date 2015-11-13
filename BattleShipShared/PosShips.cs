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

            //Infos Porte-Avion
            public Point PPorteAvion;
            public Orientation OPorteAvion;

            //Infos Croiseur
            public Point PCroiseur;
            public Orientation OCroiseur;

            //Infos Contre-Torpilleur
            public Point PContreTorpilleur;
            public Orientation OContreTorpilleur;

            //Infos Sous-Marin
            public Point PSousMarin;
            public Orientation OSousMarin;

            //Infos Torpilleur
            public Point PTorpilleur;
            public Orientation OTorpilleur;



        }
    }
}
