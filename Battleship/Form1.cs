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
                        LB_State.Text = "PlacingBoat";
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.WaitingTurn:
                        LB_State.Text = "WaitingTurn";
                        lastStat = jeu.State;
                        break;
                    case Jeu.GameState.PlayingTurn:
                        LB_State.Text = "PlayingTurn";
                        lastStat = jeu.State;
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
                                jeu.Close();
                                jeu = new Jeu();
                            }
                        }
                        //lastStat = jeu.State;
                        break;
                    case Jeu.GameState.Victory:
                        LB_State.Text = "Victory";
                        if (jeu.State != lastStat)
                        {
                            lastStat = jeu.State;
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
                Jeu.Lock.ReleaseMutex();
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (battleShipGrid1.EtatGrille == BattleShipGrid.BattleShipGrid.GridState.BateauxPlacer)
            {
                jeu.EnvoiBateau(battleShipGrid1.PositionBateau);
            }
        }

        private void BT_Connection_Click(object sender, EventArgs e)
        {
            try
            {
                jeu = new Jeu();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void battleShipGrid1_Click(object sender, EventArgs e)
        {
            Point hitPoint;
            if (jeu.State == Jeu.GameState.PlayingTurn && (hitPoint = battleShipGrid1.GetLastCoords) != null)
            {

            }
        }
    }
}
