using BattleShipShared.Packet;
using BattleShipShared.UtilityTools;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public TcpClient Joueur1 { get; private set; }
        public TcpClient Joueur2 { get; private set; }
        Thread jeu;

        public GameInstance(TcpClient client)
        {
            Joueur1 = client;
            IsWaitingForPlayer = true;
        }

        public void AjoutJoueur(TcpClient client)
        {
            if (ConnUtility.TestClient(Joueur1))
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
            finally
            {
                Joueur1 = null;
                Joueur2 = null;
            }
        }


        private void StartGame()
        {
            NetworkStream StreamJ1 = Joueur1.GetStream();
            NetworkStream StreamJ2 = Joueur2.GetStream();

            PosShips GrilleJ1 = null;
            PosShips GrilleJ2 = null;


            LogConsole.Log("Début Partie");

            CommUtility.SerializeAndSend(StreamJ1, "Start");
            CommUtility.SerializeAndSend(StreamJ2, "Start");

            StreamJ1.ReadTimeout = StreamJ2.ReadTimeout = 60000;

            //Réceptions des Grilles de jeux
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
                if (GrilleJ1 != null || GrilleJ2 != null)
                {
                    if (ConnUtility.TestClient(Joueur1))
                    {
                        CommUtility.SerializeAndSend(StreamJ1, new Result { Etat = Result.ResultState.Victory, EnemyShips = GrilleJ2 });
                    }
                    else if (ConnUtility.TestClient(Joueur2))
                    {
                        CommUtility.SerializeAndSend(StreamJ2, new Result { Etat = Result.ResultState.Victory, EnemyShips = GrilleJ1 });
                    }

                }

                StopGameInstance();

                LogConsole.Log("Erreur réception grille Erreur: " + e.Message);
            }

            //Envoie du Début partie au joueur 1
            CommUtility.SerializeAndSend(StreamJ1, new Hit
            {
                Etat = Hit.HitState.NoAction,
                Location = new Point(-1, -1)
            });

            StreamJ1.ReadTimeout = StreamJ2.ReadTimeout = 60000;
            bool partieFini = false;
            bool J1Joue = true;

            //Liste des tir reçu par chaque joueur
            List<Hit> HitJoueur1 = new List<Hit>();
            List<Hit> HitJoueur2 = new List<Hit>();

            //Boucle du Jeu
            while (!partieFini)
            {

                try
                {
                    //Attend le Hit du joueur qui joue en se moment

                    try
                    {
                        //Attend le Hit selon le joueur qui joue en se moment
                        ((!J1Joue) ? HitJoueur1 : HitJoueur2).Add((Hit)CommUtility.ReadAndDeserialize((J1Joue ? StreamJ1 : StreamJ2)));



                    }
                    catch (Exception)
                    {
                        //Si le Joueur présent ne répond plus
                        if (!ConnUtility.TestClient((J1Joue ? Joueur1 : Joueur2)))
                        {
                            //L'autre joueur gagne
                            CommUtility.SerializeAndSend((!J1Joue ? StreamJ1 : StreamJ2), new Result { Etat = Result.ResultState.Victory, EnemyShips = (J1Joue ? GrilleJ1 : GrilleJ2) });

                        }
                        else
                        {
                            //L'autre joueur gagne
                            CommUtility.SerializeAndSend((!J1Joue ? StreamJ1 : StreamJ2), new Result { Etat = Result.ResultState.Victory, EnemyShips = (J1Joue ? GrilleJ1 : GrilleJ2) });
                            //le joueur présent perd
                            CommUtility.SerializeAndSend((J1Joue ? StreamJ1 : StreamJ2), new Result { Etat = Result.ResultState.Lose, EnemyShips = (!J1Joue ? GrilleJ1 : GrilleJ2) });

                        }

                        partieFini = true;
                    }

                    //Process le hit
                    Hit nouveauHit = new Hit();
                    if (!partieFini)
                    {
                        nouveauHit.Etat = ProcessHit(((J1Joue) ? HitJoueur2 : HitJoueur1), ((J1Joue) ? GrilleJ2 : GrilleJ1));
                        nouveauHit.Location = ((J1Joue) ? HitJoueur2 : HitJoueur1).Last().Location;

                        //Envoie l'update aux joueurs

                        try
                        {
                            //Suposer envoyer l'update au joueur adverse en premier pour qu'il commence sont tour le plus vite possible
                            CommUtility.SerializeAndSend(((J1Joue) ? StreamJ2 : StreamJ1), nouveauHit);
                            CommUtility.SerializeAndSend(((!J1Joue) ? StreamJ2 : StreamJ1), nouveauHit);

                            ////Envoie Standart
                            //CommUtility.SerializeAndSend(StreamJ1, nouveauHit);
                            //CommUtility.SerializeAndSend(StreamJ2, nouveauHit);
                        }
                        catch (Exception e)
                        {
                            //Handler les erreurs

                            LogConsole.Log("Erreur lors de l'envoie des hit: " + e.Message);
                        }
                    }


                    //Check si la partie est fini


                }
                catch (Exception)
                {
                    //Une Shit load de Handling d'erreur
                    throw;
                }

            }




            /*recevoir tableau*/

            /*partir boucle Joueur1*/
            //jeu.Abort();
            //while (true)
            //{
            //    Thread.Sleep(500);

            //}

            //MatchMakingServeur.GameInstances.Remove(this);

        }

        private Hit.HitState ProcessHit(List<Hit> Hitlist, PosShips GrilleAdverse)
        {
            //Porte Avion
            if (containsHit(Hitlist.Last().Location, GrilleAdverse.PPorteAvion, GrilleAdverse.OPorteAvion, 5))
            {
                if (Hitlist.Count(touche => containsHit(touche.Location, GrilleAdverse.PPorteAvion, GrilleAdverse.OPorteAvion, 5)) == 5)
                {
                    return Hit.HitState.CoulerPorteAvion;
                }
                else
                    return Hit.HitState.Hit;
            }
            //Croiseur
            else if (containsHit(Hitlist.Last().Location, GrilleAdverse.PCroiseur, GrilleAdverse.OCroiseur, 4))
            {
                if (Hitlist.Count(touche => containsHit(touche.Location, GrilleAdverse.PCroiseur, GrilleAdverse.OCroiseur, 4)) == 4)
                {
                    return Hit.HitState.CoulerCroiseur;
                }
                else
                    return Hit.HitState.Hit;
            }
            //Contre Torpilleur
            else if (containsHit(Hitlist.Last().Location, GrilleAdverse.PContreTorpilleur, GrilleAdverse.OContreTorpilleur, 3))
            {
                if (Hitlist.Count(touche => containsHit(touche.Location, GrilleAdverse.PContreTorpilleur, GrilleAdverse.OContreTorpilleur, 3)) == 3)
                {
                    return Hit.HitState.CoulerContreTorpilleur;
                }
                else
                    return Hit.HitState.Hit;
            }
            //Sous Marin
            else if (containsHit(Hitlist.Last().Location, GrilleAdverse.PSousMarin, GrilleAdverse.OSousMarin, 3))
            {
                if (Hitlist.Count(touche => containsHit(touche.Location, GrilleAdverse.PSousMarin, GrilleAdverse.OSousMarin, 3)) == 3)
                {
                    return Hit.HitState.CoulerSousMarin;
                }
                else
                    return Hit.HitState.Hit;
            }
            //Torpilleur
            else if (containsHit(Hitlist.Last().Location, GrilleAdverse.PTorpilleur, GrilleAdverse.OTorpilleur, 2))
            {
                if (Hitlist.Count(touche => containsHit(touche.Location, GrilleAdverse.PTorpilleur, GrilleAdverse.OTorpilleur, 2)) == 2)
                {
                    return Hit.HitState.CoulerTorpilleur;
                }
                else
                    return Hit.HitState.Hit;
            }
            else
            {
                return Hit.HitState.Flop;
            }


        }

        private bool containsHit(Point Hit, Point location, PosShips.Orientation orient, int longueur)
        {
            if (orient == PosShips.Orientation.Horizontale)
            {
                if (Hit.X >= location.X && Hit.X <= location.X + longueur && Hit.Y == location.Y)
                    return true;
                else
                    return false;
            }
            else
            {
                if (Hit.Y >= location.Y && Hit.Y <= location.Y + longueur && Hit.X == location.X)
                    return true;
                else
                    return false;
            }
        }

        private bool ToucherCouler(Point Hit, Point location, PosShips.Orientation orient, int longueur)
        {
            if (orient == PosShips.Orientation.Horizontale)
            {
                if (Hit.X >= location.X && Hit.X <= location.X + longueur && Hit.Y == location.Y)
                    return true;
                else
                    return false;
            }
            else
            {
                if (Hit.Y >= location.Y && Hit.Y <= location.Y + longueur && Hit.X == location.X)
                    return true;
                else
                    return false;
            }
        }

    }
}
