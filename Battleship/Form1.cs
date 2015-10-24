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
            if(jeu!=null)
            jeu.Close();
        }

        private void KillCurrentThread()
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(jeu!=null)
            switch (jeu.State)
            {
                case Jeu.GameState.WaitingStartGame:
                    LB_State.Text = "WaitingStartGame";
                    break;
                case Jeu.GameState.PlacingBoat:
                    LB_State.Text = "PlacingBoat";
                    break;
                case Jeu.GameState.WaitingTurn:
                    LB_State.Text = "WaitingTurn";
                    break;
                case Jeu.GameState.PlayingTurn:
                    LB_State.Text = "PlayingTurn";
                    break;
                case Jeu.GameState.Victory:
                    LB_State.Text = "Victory";
                    break;
                case Jeu.GameState.Lose:
                    LB_State.Text = "Lose";
                    break;
                default:
                    LB_State.Text = "WTF";
                    break;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            jeu.EnvoiBateau();
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
    }
}
