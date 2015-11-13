using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class Form1 : Form
    {
        Jeu jeu;
        Thread thread;
        Jeu.GameState lastStat;
        public Form1()
        {
            InitializeComponent();
            timer1.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null)
            {
                thread = new Thread(jeu.AttendreDebutPartie);
                thread.Start();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            KillCurrentThread();
            if (jeu != null)
                jeu.Close();
        }

        private void KillCurrentThread()
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (jeu != null)
            {
                Jeu.Lock.WaitOne();
                
                switch (jeu.State)
                {
                    case Jeu.GameState.WaitingStartGame:
                        LB_State.Text = "Attente d'une partie";
                        lastStat = jeu.State;                        
                        break;
                    case Jeu.GameState.PlacingBoat:
                        if (jeu.State != lastStat)
                        {
                            BSG_Client.DebutPlacerBateaux();
                        }
                        LB_State.Text = "Placer vos bateaux";
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.WaitingTurn:
                        LB_State.Text = "Attente de l'autre joueur..";
                        LB_State.ForeColor = Color.Red;
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.PlayingTurn:
                        
                        LB_State.Text = "À vous de jouer !";
                        LB_State.ForeColor = Color.Green;
                        if (lastStat != jeu.State)
                        {
                            lastStat = jeu.State;
                            BSG_Enemy.WaitForinput();                        
                        }
                            
                        break;
                    case Jeu.GameState.ServerDC:
                        LB_State.Text = "Echec Du Serveur";
                        LB_State.ForeColor = Color.Red;
                        if (jeu.State != lastStat)
                        {
                            lastStat = jeu.State;
                            if (MessageBox.Show("La Connexion avec le serveur a été interrompu\nVoulez-vous Réesseyer ?",
                                "Problème Reseau",
                                MessageBoxButtons.RetryCancel,
                                MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Retry)
                            {
                                lastStat = Jeu.GameState.WaitingStartGame;
                                jeu.Close();
                                Jeu.Lock.ReleaseMutex();
                                if (TB_IpAdress.Text == "")
                                    jeu = new Jeu("LocalHost",BSG_Client.AddHit);
                                else
                                    jeu = new Jeu(TB_IpAdress.Text, BSG_Client.AddHit);
                                Jeu.Lock.WaitOne();
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        //lastStat = jeu.State;
                        break;
                    case Jeu.GameState.Victory:
                        LB_State.Text = "Victoire";
                        if (jeu.State != lastStat)
                        {
                            lastStat = jeu.State;
                            BSG_Enemy.PositionBateau = jeu.EnemyShips;
                            BSG_Enemy.Refresh();
                            MessageBox.Show("Victoire! :)",
                                                        "État de la partie",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information);
                        }
                        //lastStat = jeu.State;
                        break;
                    case Jeu.GameState.Lose:
                        LB_State.Text = "Perdu";
                        if (jeu.State != lastStat)
                        {
                            lastStat = jeu.State;
                            BSG_Enemy.PositionBateau = jeu.EnemyShips;
                            BSG_Enemy.Refresh();
                            MessageBox.Show("Défaite.. :(",
                                                      "État de la partie",
                                                      MessageBoxButtons.OK,
                                                      MessageBoxIcon.Information);
                        }
                        //lastStat = jeu.State;
                        break;
                    default:
                        LB_State.Text = "WTF";
                        lastStat = jeu.State;
                        MessageBox.Show("L'état du jeu est inconnue..",
                                                    "Oups..",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                        break;
                }
                LB_Debug.Text = BSG_Enemy.WaitingForInput.ToString();
                Jeu.Lock.ReleaseMutex();
                
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (BSG_Client.EtatGrille == BattleShipGrid.BattleShipGrid.GridState.BateauxPlacer)
            {
                jeu.EnvoiBateau(BSG_Client.PositionBateau);
                BTN_EnvoyerBateaux.Enabled = false;
            }
        }

        private void BT_Connection_Click(object sender, EventArgs e)
        {
            try
            {
                if (TB_IpAdress.Text == "")
                    jeu = new Jeu("LocalHost", BSG_Client.AddHit);
                else
                    jeu = new Jeu(TB_IpAdress.Text, BSG_Client.AddHit);
                BTN_Connection.Enabled = false;
                TB_IpAdress.Enabled = false;
                BTN_EnvoyerBateaux.Enabled = true;                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        

        private void battleShipGridAttaque1_OnHit(object sender, BattleShipGridAttaque.BattleShipGridAttaque.HitArgs args)
        {
            //BSG_Enemy.WaitingForInput = false;
            jeu.PlayingTurn(args.Location,BSG_Enemy.AddHit);
            //Thread.Sleep(100);
        }
    }
}
