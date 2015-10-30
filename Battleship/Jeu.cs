using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using BattleShipShared.Packet;
using BattleShipShared.UtilityTools;


namespace Battleship
{
    class Jeu
    {
        public enum GameState
        {
            WaitingStartGame,
            PlacingBoat,
            WaitingTurn,
            PlayingTurn,
            Victory,
            Lose
        }
        public GameState State { get; private set; }
        private TcpClient serveur;
        private Thread Attente;
        //private volatile bool gameStarted = false;
        public Jeu()
        {
            State = GameState.WaitingStartGame;
            serveur = new TcpClient("P104-14", 8080);
            Attente = new Thread(AttendreDebutPartie);
            Attente.Start();
        }
        public void AttendreDebutPartie()
        {
            try
            {
                NetworkStream ns = serveur.GetStream();

                do
                {
                    //Console.Beep(700, 200);
                    continue;
                } while ((String)CommUtility.ReadAndDeserialize(ns) != "Start");

                //gameStarted = true;
                State = GameState.PlacingBoat;

            }
            catch (ThreadAbortException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public void EnvoiBateau(PosShips grilleBeto)
        {
            //CommUtility.SerializeAndSend(serveur.GetStream(), new PosShips
            //{
            //    PPorteAvion = new Point(0, 0),
            //    OPorteAvion = PosShips.Orientation.Horizontale,
            //    PCroiseur = new Point(0, 1),
            //    OCroiseur = PosShips.Orientation.Horizontale,
            //    PContreTorpilleur = new Point(0, 2),
            //    OContreTorpilleur = PosShips.Orientation.Horizontale,
            //    PSousMarin = new Point(0, 3),
            //    OSousMarin = PosShips.Orientation.Horizontale,
            //    PTorpilleur = new Point(0, 4),
            //    OTorpilleur = PosShips.Orientation.Horizontale
            //});
            CommUtility.SerializeAndSend(serveur.GetStream(), grilleBeto);
            State = Jeu.GameState.WaitingTurn;
        }
        public void Close()
        {
            Attente.Abort();
            serveur.Close();

        }
    }
}
