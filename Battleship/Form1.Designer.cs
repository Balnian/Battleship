namespace Battleship
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LB_State = new System.Windows.Forms.Label();
            this.BTN_EnvoyerBateaux = new System.Windows.Forms.Button();
            this.BTN_Connection = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lb_takeInputs = new System.Windows.Forms.Label();
            this.BSG_Client = new BattleShipGrid.BattleShipGrid();
            this.BSG_Enemy = new BattleShipGridAttaque.BattleShipGridAttaque();
            this.TB_IpAdress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 5;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LB_State
            // 
            this.LB_State.AutoEllipsis = true;
            this.LB_State.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_State.Location = new System.Drawing.Point(345, 351);
            this.LB_State.Name = "LB_State";
            this.LB_State.Size = new System.Drawing.Size(304, 35);
            this.LB_State.TabIndex = 2;
            this.LB_State.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_EnvoyerBateaux
            // 
            this.BTN_EnvoyerBateaux.Enabled = false;
            this.BTN_EnvoyerBateaux.Location = new System.Drawing.Point(130, 351);
            this.BTN_EnvoyerBateaux.Name = "BTN_EnvoyerBateaux";
            this.BTN_EnvoyerBateaux.Size = new System.Drawing.Size(91, 71);
            this.BTN_EnvoyerBateaux.TabIndex = 3;
            this.BTN_EnvoyerBateaux.Text = "Envoyer Bateau";
            this.BTN_EnvoyerBateaux.UseVisualStyleBackColor = true;
            this.BTN_EnvoyerBateaux.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // BTN_Connection
            // 
            this.BTN_Connection.Location = new System.Drawing.Point(6, 18);
            this.BTN_Connection.Name = "BTN_Connection";
            this.BTN_Connection.Size = new System.Drawing.Size(100, 23);
            this.BTN_Connection.TabIndex = 4;
            this.BTN_Connection.Text = "Connecter";
            this.BTN_Connection.UseVisualStyleBackColor = true;
            this.BTN_Connection.Click += new System.EventHandler(this.BT_Connection_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 28);
            this.label1.TabIndex = 7;
            this.label1.Text = "Position Local";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(389, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 28);
            this.label2.TabIndex = 8;
            this.label2.Text = "Position Ennemie";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(325, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 300);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // lb_takeInputs
            // 
            this.lb_takeInputs.AutoSize = true;
            this.lb_takeInputs.Location = new System.Drawing.Point(249, 409);
            this.lb_takeInputs.Name = "lb_takeInputs";
            this.lb_takeInputs.Size = new System.Drawing.Size(26, 13);
            this.lb_takeInputs.TabIndex = 11;
            this.lb_takeInputs.Text = "État";
            // 
            // BSG_Client
            // 
            this.BSG_Client.BorderOfSelection = System.Drawing.Color.Transparent;
            this.BSG_Client.ContreTorpilleur = global::Battleship.Properties.Resources.contretorpilleur;
            this.BSG_Client.Croiseur = global::Battleship.Properties.Resources.Croiseur;
            this.BSG_Client.GridColor = System.Drawing.Color.Black;
            this.BSG_Client.GridNumber = ((uint)(10u));
            this.BSG_Client.InteriorOfSelection = System.Drawing.Color.Red;
            this.BSG_Client.Location = new System.Drawing.Point(12, 39);
            this.BSG_Client.Name = "BSG_Client";
            this.BSG_Client.PorteAvions = global::Battleship.Properties.Resources.PorteAvions;
            this.BSG_Client.Size = new System.Drawing.Size(300, 300);
            this.BSG_Client.SousMarin = global::Battleship.Properties.Resources.SousMarin;
            this.BSG_Client.TabIndex = 5;
            this.BSG_Client.Torpilleur = global::Battleship.Properties.Resources.torpilleur;
            // 
            // BSG_Enemy
            // 
            this.BSG_Enemy.BorderOfSelection = System.Drawing.Color.Transparent;
            this.BSG_Enemy.ContreTorpilleur = global::Battleship.Properties.Resources.contretorpilleur;
            this.BSG_Enemy.Croiseur = global::Battleship.Properties.Resources.Croiseur;
            this.BSG_Enemy.GridColor = System.Drawing.Color.Black;
            this.BSG_Enemy.GridNumber = ((uint)(10u));
            this.BSG_Enemy.InteriorOfSelection = System.Drawing.Color.Red;
            this.BSG_Enemy.Location = new System.Drawing.Point(349, 39);
            this.BSG_Enemy.Name = "BSG_Enemy";
            this.BSG_Enemy.PorteAvions = global::Battleship.Properties.Resources.PorteAvions;
            this.BSG_Enemy.PositionBateau = null;
            this.BSG_Enemy.Size = new System.Drawing.Size(300, 300);
            this.BSG_Enemy.SousMarin = global::Battleship.Properties.Resources.SousMarin;
            this.BSG_Enemy.TabIndex = 10;
            this.BSG_Enemy.Torpilleur = global::Battleship.Properties.Resources.torpilleur;
            this.BSG_Enemy.WaitingForInput = false;
            this.BSG_Enemy.OnHit += new BattleShipGridAttaque.BattleShipGridAttaque.HitHandler(this.battleShipGridAttaque1_OnHit);
            // 
            // TB_IpAdress
            // 
            this.TB_IpAdress.Location = new System.Drawing.Point(6, 48);
            this.TB_IpAdress.Name = "TB_IpAdress";
            this.TB_IpAdress.Size = new System.Drawing.Size(100, 20);
            this.TB_IpAdress.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BTN_Connection);
            this.groupBox1.Controls.Add(this.TB_IpAdress);
            this.groupBox1.Location = new System.Drawing.Point(12, 345);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 77);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connexion";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 448);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lb_takeInputs);
            this.Controls.Add(this.BSG_Enemy);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BSG_Client);
            this.Controls.Add(this.BTN_EnvoyerBateaux);
            this.Controls.Add(this.LB_State);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label LB_State;
        private System.Windows.Forms.Button BTN_EnvoyerBateaux;
        private System.Windows.Forms.Button BTN_Connection;
        private BattleShipGrid.BattleShipGrid BSG_Client;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private BattleShipGridAttaque.BattleShipGridAttaque BSG_Enemy;
        private System.Windows.Forms.Label lb_takeInputs;
        private System.Windows.Forms.TextBox TB_IpAdress;
        private System.Windows.Forms.GroupBox groupBox1;

    }
}

