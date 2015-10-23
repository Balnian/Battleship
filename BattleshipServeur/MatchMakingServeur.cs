using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipServer
{
    class MatchMakingServeur
    {
        public static List<GameInstance> GameInstances = new List<GameInstance>();

        public bool Stop { get; set; }
        private Mutex Lock;
        public static Thread Nettoyage;

        public MatchMakingServeur(Mutex Lock)
        {
            this.Lock = Lock;
            Nettoyage = new Thread(cleanInstances);//100ms / Instances
            Nettoyage.Start();
        }

        public void ListenServeur()
        {
            Stop = false;
            //TcpListener serverSocket = new TcpListener(8888);
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 8080);
            try
            {

                serverSocket.Server.ReceiveTimeout = 500;
                serverSocket.Start();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            //TcpClient clientSocket = default(TcpClient);


            LogConsole.LogWithTime("Serveur à démarrer");
            while (!Stop)
            {

                if (serverSocket.Pending())
                {
                    TcpClient clientSocket = serverSocket.AcceptTcpClient();

                    LogConsole.LogWithTime("Nouvelle connection de " + ConnUtility.GetIP(clientSocket)/* IPAddress.Parse(((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString())*/);
                    Lock.WaitOne();
                    if (CheckExistingInstances(clientSocket))
                    {
                        GameInstances.Add(new GameInstance(clientSocket));
                    }
                    Lock.ReleaseMutex();
                }
               
            }
            LogConsole.LogWithTime("Serveur s'est arrêter");
            serverSocket.Stop();

        }

        private bool CheckExistingInstances(TcpClient client)
        {

            if (GameInstances.Count>0 && GameInstances.Last().IsWaitingForPlayer)
            {
                GameInstances.Last().AjoutJoueur(client);
                return false;
            }
            //foreach (GameInstance game in GameInstances)
            //{
            //    if (game.IsWaitingForPlayer)
            //    {
            //        game.AjoutJoueur(client);
            //        return false;

            //    }
            //}
            
            return true;
        }

        private void cleanInstances()
        {
            int pos = 0;
            while (true)
            {
                //Lock   
                Lock.WaitOne();
                if (GameInstances.Count > 0)
                {


                    if (!ConnUtility.TestClient(GameInstances.ElementAt(pos).Joueur2) && !ConnUtility.TestClient(GameInstances.ElementAt(pos).Joueur1))
                    {
                        GameInstances.Remove(GameInstances.ElementAt(pos));
                    }
                    else
                    {
                        pos++;
                    }
                }
                if (pos >= GameInstances.Count)
                    pos = 0;
                //Unlock
                Lock.ReleaseMutex();

            }
            //List<GameInstance> Rip = new List<GameInstance>();
            ////ici
            //foreach (GameInstance instance in GameInstances)
            //{
            //    if (!ConnUtility.TestClient(instance.Joueur2) && !ConnUtility.TestClient(instance.Joueur1))
            //        Rip.Add(instance);

            //    /*if ((instance.Joueur1 == null || !instance.Joueur1.Connected) && (instance.Joueur2 == null || !instance.Joueur2.Connected))
            //    {

            //            GameInstances.Remove(instance);
            //    }
            //    else if(instance.Joueur1 != null && instance.Joueur2 != null)
            //        if (!testconn(instance.Joueur2) && !testconn(instance.Joueur1))
            //            GameInstances.Remove(instance);*/

            //}

            //foreach (GameInstance instance in Rip)
            //{
            //    GameInstances.Remove(instance);
            //}
        }



        private void deleteInstance(GameInstance instance)
        {
            instance.StopGameInstance();
            GameInstances.Remove(instance);
        }

        private void LogWithTime(String Message)
        {
            Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ">> " + Message);
        }
    }
}
