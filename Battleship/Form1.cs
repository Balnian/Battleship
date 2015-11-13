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
                        LB_State.Text = "WaitingStartGame";
                        lastStat = jeu.State;                        
                        break;
                    case Jeu.GameState.PlacingBoat:
                        if (jeu.State != lastStat)
                        {
                            BSG_Client.DebutPlacerBateaux();
                        }
                        LB_State.Text = "PlacingBoat";
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.WaitingTurn:
                        LB_State.Text = "WaitingTurn";
                        LB_State.ForeColor = Color.Red;
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.PlayingTurn:
                        
                        LB_State.Text = "PlayingTurn";
                        LB_State.ForeColor = Color.Green;
                        if (lastStat != jeu.State)
                        {
                            lastStat = jeu.State;
                            BSG_Enemy.WaitingForInput = true;                         
                        }
                            
                        break;
                    case Jeu.GameState.ServerDC:
                        LB_State.Text = "ServerDC";
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
                                    jeu = new Jeu("LocalHost");
                                else
                                    jeu = new Jeu(TB_IpAdress.Text);
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
                        LB_State.Text = "Victory";
                        if (jeu.State != lastStat)
                        {
                            lastStat = jeu.State;
                            BSG_Enemy.PositionBateau = jeu.EnemyShips;
                            MessageBox.Show("Victoire! :)",
                                                        "État de la partie",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information);
                        }
                        //lastStat = jeu.State;
                        break;
                    case Jeu.GameState.Lose:
                        LB_State.Text = "Lose";
                        if (jeu.State != lastStat)
                        {
                            lastStat = jeu.State;
                            BSG_Enemy.PositionBateau = jeu.EnemyShips;
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
                lb_takeInputs.Text = BSG_Enemy.WaitingForInput.ToString();
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
                    jeu = new Jeu("LocalHost");
                else
                    jeu = new Jeu(TB_IpAdress.Text);
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
