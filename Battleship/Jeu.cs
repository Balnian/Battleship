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
        //private volatile bool gameStarted = false;

        public Jeu()
        {
            Lock.WaitOne();
            State = GameState.WaitingStartGame;
            Lock.ReleaseMutex();
            UpdateAction();

        }

        private void UpdateAction()
        {
            Lock.WaitOne();
            switch (State)
            {
                case GameState.WaitingStartGame:
                    try
                    {
                        serveur = new TcpClient(/*"173.178.211.254"*/"P104-13", 8080);
                        attente = new Thread(AttendreDebutPartie);
                        attente.Start();
                    }
                    catch (Exception es)
                    {
                        Console.Beep(800, 700);
                        State = GameState.ServerDC;
                        //UpdateAction();
                    }
                    
                    break;
                case GameState.PlacingBoat:
                    //L'utilisateur place ses bateau dans l'interface
                    break;
                case GameState.WaitingTurn:
                    waitingTurn = new Thread(WaitingTurn);
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
            Lock.WaitOne();
            State = Jeu.GameState.WaitingTurn;
            Lock.ReleaseMutex();
            UpdateAction();

        }

        public void WaitingTurn()
        {
            object data = null;
            try
            {
                serveur.ReceiveTimeout = 90000;
                data = CommUtility.ReadAndDeserialize(serveur.GetStream());
                Hit hit = (Hit)data;
                if (hit.Etat != Hit.HitState.NoAction)
                    listHit.Add(hit);

                Lock.WaitOne();
                State = GameState.PlayingTurn;
                Lock.ReleaseMutex();
                UpdateAction();
            }
            catch (Exception ex)
            {
                try
                {
                    Result result = (Result)data;
                    if (result.Etat == Result.ResultState.Lose)
                    {
                        Lock.WaitOne();
                        State = GameState.Lose;
                        EnemyShips = result.EnemyShips;
                        Lock.ReleaseMutex();
                        
                    }
                    else
                    {
                        Lock.WaitOne();
                        State = GameState.Victory;
                        EnemyShips = result.EnemyShips;
                        Lock.ReleaseMutex();
                    }
                    UpdateAction();
                }
                catch (Exception es)
                {
                    Lock.WaitOne();
                    State = GameState.ServerDC;
                    Lock.ReleaseMutex();
                    UpdateAction();
                }
            }
        }

        public delegate void func(Hit leH);
        public void PlayingTurn(Point point, func AjoutHit)
        {
            CommUtility.SerializeAndSend(serveur.GetStream(), new Hit { Etat = Hit.HitState.NoAction, Location = point });

            (new Thread(() => waitHitConfirm(AjoutHit))).Start();


        }



        private void waitHitConfirm(func AjoutHit)
        {
            object carry = null;
            try
            {
                carry = CommUtility.ReadAndDeserialize(serveur.GetStream());
                AjoutHit((Hit)carry);
                Lock.WaitOne();
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
                        if(result.Etat == Result.ResultState.Victory)
                        {
                            Lock.WaitOne();
                            State = GameState.Victory;
                            EnemyShips = result.EnemyShips;
                            Lock.ReleaseMutex();
                            UpdateAction();

                        }
                        else
                        {
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
