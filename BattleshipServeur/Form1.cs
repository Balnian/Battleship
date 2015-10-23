using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleshipServer
{
    //enum ServerState
    //{
    //    Arreter,
    //    Demarrer,
    //    Pause
    //}
    public partial class Form1 : Form
    {
        MatchMakingServeur serv;
        Thread Serveur;
        Mutex LockRessource;

       // ServerState State = ServerState.Arreter;
        public Form1()
        {
            InitializeComponent();
            AllocConsole();
            LockRessource = new Mutex();
            serv = new MatchMakingServeur(LockRessource);
            Serveur = new Thread(serv.ListenServeur);
            timer1.Start();
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = MatchMakingServeur.GameInstances.Count.ToString();
            UpdateGameInstancesView();
        }

        private void RB_Arreter_CheckedChanged(object sender, EventArgs e)
        {
            if(RB_Arreter.Checked)
            ArreterServeur();
        }

        private void RB_Demarrer_CheckedChanged(object sender, EventArgs e)
        {
            if(RB_Demarrer.Checked)
                DemarrerServeur();
        }

        private void RB_Pause_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ArreterServeur()
        {
            serv.Stop = true;
            /*if (Serveur.ThreadState == ThreadState.Running)
            {
                Serveur.Abort();
            }*/
            
        }

        private void DemarrerServeur()
        {
           /* switch (Serveur.ThreadState)
            {
                case ThreadState.AbortRequested:
                    break;
                case ThreadState.Aborted:
                    Serveur = new Thread(serv.ListenServeur);

                
                case ThreadState.Unstarted:
                    Serveur.Start();

                    break;
                default:
                    
                    break;
            }*/
            if (Serveur.ThreadState==ThreadState.Stopped)
            {
                Serveur = new Thread(serv.ListenServeur);
            }
           
           Serveur.Start();

	       
            
            
        }

        void UpdateGameInstancesView()
        {
            int compteurJoueur = 0;
            int compteurNode = 0;

            LockRessource.WaitOne();
            foreach (GameInstance Instance in MatchMakingServeur.GameInstances)
            {
                if (Instance.Joueur1 != null)
                {
                    compteurJoueur++;
                }
                if (Instance.Joueur2 != null)
                {
                    compteurJoueur++;
                }

            }

            foreach (TreeNode node in TV_GameInstancesView.Nodes)
            {
                compteurNode += node.Nodes.Count;
            }

            if (compteurJoueur != compteurNode)
            {

                
                TV_GameInstancesView.Nodes.Clear();
                
                /*foreach (TreeNode node in TV_GameInstancesView.Nodes)
                {
                    node.Remove();
                }
                */
                for (int i = 0; i < MatchMakingServeur.GameInstances.Count; i++)
                {
                    TreeNode currNode = TV_GameInstancesView.Nodes.Add("Instance" + i.ToString(),"Instance" + i.ToString(), "Instance");
                    currNode.SelectedImageKey = "Instance";

                    currNode.Nodes.Add("Joueur1",((IPEndPoint)MatchMakingServeur.GameInstances[i].Joueur1.Client.RemoteEndPoint).Address.ToString(),"Joueur")
                        .SelectedImageKey = "Joueur";
                    if (MatchMakingServeur.GameInstances[i].Joueur2 != null)
                    {
                        currNode.Nodes.Add("Joueur1", ((IPEndPoint)MatchMakingServeur.GameInstances[i].Joueur2.Client.RemoteEndPoint).Address.ToString(), "Joueur")
                            .SelectedImageKey = "Joueur";
                    }
                   
                        
                    
                }
            }
            LockRessource.ReleaseMutex();
        }




    }
}
