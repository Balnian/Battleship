﻿using System;
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

        /// <summary>
        /// Tick timer
        /// Sert a mettre a jour les états du jeu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (jeu != null)
            {
                Jeu.Lock.WaitOne();

                switch (jeu.State)
                {
                    case Jeu.GameState.WaitingStartGame://Attend d'être jumellé
                        LB_State.Text = "Attente d'une partie";
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.PlacingBoat://on place les bateaux
                        if (jeu.State != lastStat)
                        {
                            BSG_Client.DebutPlacerBateaux();
                        }
                        LB_State.Text = "Placer vos bateaux";
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.WaitingTurn://En attente pour jouer
                        LB_State.Text = "Attente de l'autre joueur..";
                        LB_State.ForeColor = Color.Red;
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.PlayingTurn://notre tour de jouer

                        LB_State.Text = "À vous de jouer !";
                        LB_State.ForeColor = Color.Green;
                        if (lastStat != jeu.State)
                        {
                            lastStat = jeu.State;
                            BSG_Enemy.WaitForinput();
                        }

                        break;
                    case Jeu.GameState.ServerDC://État d'érreur par rapport a la connexion
                        LB_State.Text = "Echec Du Serveur";
                        LB_State.ForeColor = Color.Red;
                        if (jeu.State != lastStat)
                        {
                            lastStat = jeu.State;
                            MessageBox.Show("La Connexion avec le serveur a été interrompu",
                                "Problème Reseau",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            this.Close();
                        }
                        //lastStat = jeu.State;
                        break;
                    case Jeu.GameState.Victory://Victoire
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
                    case Jeu.GameState.Lose://Défaite
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
                    default://état inconnue, action par DÉFAUT
                        LB_State.Text = "WTF";//
                        lastStat = jeu.State;
                        MessageBox.Show("L'état du jeu est inconnue..",
                                                    "Oups..",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                        break;
                }
                Jeu.Lock.ReleaseMutex();

            }

        }

        /// <summary>
        /// Evennement Click boutton envoyer bateau
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_EnvoyerBateau_Click_1(object sender, EventArgs e)
        {
            if (BSG_Client.EtatGrille == BattleShipGrid.BattleShipGrid.GridState.BateauxPlacer)
            {
                jeu.EnvoiBateau(BSG_Client.PositionBateau);
                //Mise a jour de l'état des bouttons
                BTN_EnvoyerBateaux.Enabled = false;
            }
        }

        /// <summary>
        /// Evennement Click boutton Connexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_Connection_Click(object sender, EventArgs e)
        {
            try
            {
                if (TB_IpAdress.Text == "")//Si rien est entre, l'adresse par défaut est LocalHost
                    jeu = new Jeu("LocalHost", BSG_Client.AddHit);//nouvelle instence de JEU
                else//L'adresse passé est utilisé
                    jeu = new Jeu(TB_IpAdress.Text, BSG_Client.AddHit);//nouvelle instence de JEU
                //Mise a jour de l'état des bouttons
                BTN_Connection.Enabled = false;
                TB_IpAdress.Enabled = false;
                BTN_EnvoyerBateaux.Enabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Evennement OnHit du UserControl BattleShipGridAttaque
        /// sert a recevoir un evenemnt lors d'un click dans la grille
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void battleShipGridAttaque1_OnHit(object sender, BattleShipGridAttaque.BattleShipGridAttaque.HitArgs args)
        {
            jeu.PlayingTurn(args.Location, BSG_Enemy.AddHit);
        }
    }
}
