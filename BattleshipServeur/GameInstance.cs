using BattleShipShared.Packet;
using BattleShipShared.UtilityTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipServer
{
    class GameInstance
    {
        public bool IsWaitingForPlayer;

        public TcpClient Joueur1{get;private set;}
        public TcpClient Joueur2{get;private set;}
        Thread jeu;

        public GameInstance(TcpClient client)
        {
            Joueur1 = client;
            IsWaitingForPlayer = true;
        }

        public void AjoutJoueur(TcpClient client)
        {
            if(ConnUtility.TestClient(Joueur1))
            {

                Joueur2 = client;
                IsWaitingForPlayer = false;
                jeu = new Thread(StartGame);
                jeu.Start();
            }
            else
            {
                LogConsole.LogWithTime("La connection à " + ConnUtility.GetIP(Joueur1) + " à été terminer");
                Joueur1 = client;
            }
        }

        public void StopGameInstance()
        {
            try
            {
                jeu.Abort();
            }
            catch (Exception)
            {

                
            }
        }


        private void StartGame()
        {
            NetworkStream StreamJ1 = Joueur1.GetStream();
            NetworkStream StreamJ2 = Joueur2.GetStream();

            PosShips GrilleJ1;
            PosShips GrilleJ2;


            LogConsole.Log("Début Partie");

            CommUtility.SerializeAndSend(StreamJ1, "Start");
            CommUtility.SerializeAndSend(StreamJ2, "Start");

            StreamJ1.ReadTimeout = StreamJ2.ReadTimeout=60000;
            try 
	        {
                LogConsole.Log("Lecture 1 ");
                GrilleJ1 = (PosShips)CommUtility.ReadAndDeserialize(StreamJ1);
                LogConsole.Log("Lecture 2 ");
                GrilleJ2 = (PosShips)CommUtility.ReadAndDeserialize(StreamJ2);
                LogConsole.Log("Tentative Lecture");
                LogConsole.Log(GrilleJ1.PPorteAvion.ToString());
                LogConsole.Log(GrilleJ2.PPorteAvion.ToString());
	        }
	        catch (Exception e)
	        {
		
		        LogConsole.Log("Erreur réception grille Erreur: " + e.Message);
	        }
            


            
            
            
            /*recevoir tableau*/

            /*partir boucle Joueur1*/
            //jeu.Abort();
            while (true)
            {
                Thread.Sleep(500);

            }

            MatchMakingServeur.GameInstances.Remove(this);

        }

    }
}
