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
        /// <summary>
        /// Enum des états de Jeu(GameState)
        /// </summary>
        public enum GameState
        {
            WaitingStartGame,
            PlacingBoat,
            WaitingTurn,
            PlayingTurn,
            ServerDC,
            Victory,
            Lose
        }

        public static Mutex Lock = new Mutex();
        public GameState State { get; private set; }
        private TcpClient serveur;
        private Thread attente;
        private Thread waitingTurn;
        private List<Hit> listHit = new List<Hit>();
        public PosShips EnemyShips = null;
        public String ipAdress;
        public delegate void func(Hit leH);

        private func AddHitSelf;
        //private volatile bool gameStarted = false;

        /// <summary>
        /// Constructeur du jeu
        /// applique l'adresse IP et change l'état du jeu a WaitingStart
        /// </summary>
        /// <param name="Ip">Adresse IP</param>
        /// <param name="HandleClient"></param>
        public Jeu(String Ip,func HandleClient)
        {
            ipAdress = Ip;
            Lock.WaitOne();
            State = GameState.WaitingStartGame;
            Lock.ReleaseMutex();
            UpdateAction();
            AddHitSelf = HandleClient;

        }

        /// <summary>
        /// effectue des actions selon l'état du Jeu
        /// </summary>
        private void UpdateAction()
        {
            Lock.WaitOne();
            switch (State)
            {
                case GameState.WaitingStartGame:
                    try //Tentative de connection a l'addresse fourni
                    {
                        serveur = new TcpClient(ipAdress, 8080);
                        attente = new Thread(AttendreDebutPartie);
                        attente.Start();
                    }
                    catch (Exception es)
                    {
                        State = GameState.ServerDC;
                    }
                    
                    break;
                case GameState.PlacingBoat:
                    //L'utilisateur place ses bateau dans l'interface
                    break;
                case GameState.WaitingTurn:
                    waitingTurn = new Thread(WaitingTurn);//Part un Thread d'écoute pour attendre son tour
                    waitingTurn.Start();
                    break;
                case GameState.PlayingTurn:
                    //L'utilisateur doit jouer son tour
                    break;
                case GameState.ServerDC:
                    break;
                case GameState.Victory:
                    break;
                case GameState.Lose:
                    break;
                default:
                    break;
            }
            Lock.ReleaseMutex();
        }

        /// <summary>
        /// Attendre le réponse du serveur pour continuer
        /// </summary>
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
                Lock.WaitOne();
                State = GameState.PlacingBoat;
                Lock.ReleaseMutex();
                UpdateAction();

            }
            catch (ThreadAbortException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Envoie La grille de bateaux au servers
        /// </summary>
        /// <param name="grilleBeto"></param>
        public void EnvoiBateau(PosShips grilleBeto)
        {
            CommUtility.SerializeAndSend(serveur.GetStream(), grilleBeto);
            Lock.WaitOne();
            State = Jeu.GameState.WaitingTurn;
            Lock.ReleaseMutex();
            UpdateAction();

        }

        /// <summary>
        /// Attend son tour en Thread pour continuer a jouer
        /// </summary>
        public void WaitingTurn()
        {
            object data = null;
            try //Essai de convertir l'objet recu en Hit
            {
                serveur.ReceiveTimeout = 90000;
                data = CommUtility.ReadAndDeserialize(serveur.GetStream());
                Hit hit = (Hit)data;
                if (hit.Etat != Hit.HitState.NoAction)
                    AddHitSelf(hit);

                Lock.WaitOne();
                State = GameState.PlayingTurn;
                Lock.ReleaseMutex();
                UpdateAction();
            }
            catch (Exception ex)
            {
                try //Si pas convertable en Hit, essai de convertir en result
                {
                    Result result = (Result)data;
                    if (result.Etat == Result.ResultState.Lose)//si le résultat est "Lose"
                    {
                        if (result.Touche != null && result.Touche.Etat!=Hit.HitState.NoAction)
                            AddHitSelf(result.Touche);
                        Lock.WaitOne();
                        State = GameState.Lose;
                        EnemyShips = result.EnemyShips;
                        Lock.ReleaseMutex();
                        
                    }
                    else//si le résultat est "WIN"
                    {
                        if (result.Touche != null && result.Touche.Etat != Hit.HitState.NoAction)
                            AddHitSelf(result.Touche);
                        Lock.WaitOne();
                        State = GameState.Victory;
                        EnemyShips = result.EnemyShips;
                        Lock.ReleaseMutex();
                    }
                    UpdateAction();
                }
                catch (Exception es)//Si non convertable, on lance une érreur
                {
                    Lock.WaitOne();
                    State = GameState.ServerDC;
                    Lock.ReleaseMutex();
                    UpdateAction();
                }
            }
        }

        /// <summary>
        /// Jouer son tour
        /// Envoie d'un Hit au serveur
        /// </summary>
        /// <param name="point">point touché</param>
        /// <param name="AjoutHit"></param>
        public void PlayingTurn(Point point, func AjoutHit)
        {
            try
            {
                CommUtility.SerializeAndSend(serveur.GetStream(), new Hit { Etat = Hit.HitState.NoAction, Location = point });

                (new Thread(() => waitHitConfirm(AjoutHit))).Start();
            }
            catch (Exception)
            {

                Lock.WaitOne();
                State = GameState.ServerDC;
                Lock.ReleaseMutex();
            }
            


        }


        /// <summary>
        /// Attente de la comfirmation du serveur pour savoir si le Hit
        /// est un Flop ou un Touché
        /// </summary>
        /// <param name="AjoutHit"></param>
        private void waitHitConfirm(func AjoutHit)
        {
            object carry = null;
            try
            {
                carry = CommUtility.ReadAndDeserialize(serveur.GetStream());
                AjoutHit((Hit)carry);
                
                Lock.WaitOne();
                Thread.Sleep(300);
                State = GameState.WaitingTurn;
                Lock.ReleaseMutex();
                UpdateAction();

            }
            catch(Exception e)
            {
                try
                {
                    if (carry != null)
                    {
                        Result result = (Result)carry;
                        if(result.Etat == Result.ResultState.Victory)//Si la réponse est Victoire
                        {
                            if (result.Touche != null && result.Touche.Etat != Hit.HitState.NoAction)
                                AjoutHit(result.Touche);
                            Lock.WaitOne();
                            State = GameState.Victory;
                            EnemyShips = result.EnemyShips;
                            Lock.ReleaseMutex();
                            UpdateAction();

                        }
                        else//Si la réponse est Perdage
                        {
                            if (result.Touche != null && result.Touche.Etat != Hit.HitState.NoAction)
                                AjoutHit(result.Touche);
                            Lock.WaitOne();
                            State = GameState.Lose;
                            EnemyShips = result.EnemyShips;
                            Lock.ReleaseMutex();
                            UpdateAction();
                        }
                    }
                    else
                        throw;
                    
                }
                catch (Exception)
                {
                    Lock.WaitOne();
                    State = GameState.ServerDC;
                    Lock.ReleaseMutex();
                    UpdateAction();
                }
            }
            
        }

        /// <summary>
        /// Fermeture de tout activité avant la fermeture complete du Form
        /// </summary>
        public void Close()
        {
            if (attente != null && attente.IsAlive)
                attente.Abort();
            if (waitingTurn != null && waitingTurn.IsAlive)
                waitingTurn.Abort();
            if (serveur != null)
                serveur.Close();
        }
    }
}
