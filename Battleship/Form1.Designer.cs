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
            this.button1 = new System.Windows.Forms.Button();
            this.BT_Connection = new System.Windows.Forms.Button();
            this.battleShipGrid1 = new BattleShipGrid.BattleShipGrid();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LB_State
            // 
            this.LB_State.AutoSize = true;
            this.LB_State.Location = new System.Drawing.Point(207, 359);
            this.LB_State.Name = "LB_State";
            this.LB_State.Size = new System.Drawing.Size(26, 13);
            this.LB_State.TabIndex = 2;
            this.LB_State.Text = "État";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(110, 354);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Envoyer Bateau";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // BT_Connection
            // 
            this.BT_Connection.Location = new System.Drawing.Point(29, 354);
            this.BT_Connection.Name = "BT_Connection";
            this.BT_Connection.Size = new System.Drawing.Size(75, 23);
            this.BT_Connection.TabIndex = 4;
            this.BT_Connection.Text = "Connection";
            this.BT_Connection.UseVisualStyleBackColor = true;
            this.BT_Connection.Click += new System.EventHandler(this.BT_Connection_Click);
            // 
            // battleShipGrid1
            // 
            this.battleShipGrid1.BorderOfSelection = System.Drawing.Color.Transparent;
            this.battleShipGrid1.ContreTorpilleur = global::Battleship.Properties.Resources.contretorpilleur;
            this.battleShipGrid1.Croiseur = global::Battleship.Properties.Resources.Croiseur;
            this.battleShipGrid1.GridColor = System.Drawing.Color.Black;
            this.battleShipGrid1.GridNumber = ((uint)(10u));
            this.battleShipGrid1.InteriorOfSelection = System.Drawing.Color.Red;
            this.battleShipGrid1.Location = new System.Drawing.Point(12, 39);
            this.battleShipGrid1.Name = "battleShipGrid1";
            this.battleShipGrid1.PorteAvions = global::Battleship.Properties.Resources.PorteAvions;
            this.battleShipGrid1.Size = new System.Drawing.Size(300, 300);
            this.battleShipGrid1.SousMarin = global::Battleship.Properties.Resources.SousMarin;
            this.battleShipGrid1.TabIndex = 5;
            this.battleShipGrid1.Torpilleur = global::Battleship.Properties.Resources.torpilleur;
            this.battleShipGrid1.Click += new System.EventHandler(this.battleShipGrid1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 462);
            this.Controls.Add(this.battleShipGrid1);
            this.Controls.Add(this.BT_Connection);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LB_State);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label LB_State;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BT_Connection;
        private BattleShipGrid.BattleShipGrid battleShipGrid1;

    }
}

