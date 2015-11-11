namespace BattleshipServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RB_Demarrer = new System.Windows.Forms.RadioButton();
            this.RB_Arreter = new System.Windows.Forms.RadioButton();
            this.TV_GameInstancesView = new System.Windows.Forms.TreeView();
            this.IL_TreeView = new System.Windows.Forms.ImageList(this.components);
            this.BT_KillAllGames = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RB_Demarrer);
            this.groupBox1.Controls.Add(this.RB_Arreter);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 69);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // RB_Demarrer
            // 
            this.RB_Demarrer.AutoSize = true;
            this.RB_Demarrer.Location = new System.Drawing.Point(20, 44);
            this.RB_Demarrer.Name = "RB_Demarrer";
            this.RB_Demarrer.Size = new System.Drawing.Size(68, 17);
            this.RB_Demarrer.TabIndex = 1;
            this.RB_Demarrer.TabStop = true;
            this.RB_Demarrer.Text = "Démarrer";
            this.RB_Demarrer.UseVisualStyleBackColor = true;
            this.RB_Demarrer.CheckedChanged += new System.EventHandler(this.RB_Demarrer_CheckedChanged);
            // 
            // RB_Arreter
            // 
            this.RB_Arreter.AutoSize = true;
            this.RB_Arreter.Location = new System.Drawing.Point(20, 20);
            this.RB_Arreter.Name = "RB_Arreter";
            this.RB_Arreter.Size = new System.Drawing.Size(56, 17);
            this.RB_Arreter.TabIndex = 0;
            this.RB_Arreter.TabStop = true;
            this.RB_Arreter.Text = "Arrêter";
            this.RB_Arreter.UseVisualStyleBackColor = true;
            this.RB_Arreter.CheckedChanged += new System.EventHandler(this.RB_Arreter_CheckedChanged);
            // 
            // TV_GameInstancesView
            // 
            this.TV_GameInstancesView.Dock = System.Windows.Forms.DockStyle.Right;
            this.TV_GameInstancesView.ImageIndex = 0;
            this.TV_GameInstancesView.ImageList = this.IL_TreeView;
            this.TV_GameInstancesView.Location = new System.Drawing.Point(646, 0);
            this.TV_GameInstancesView.Name = "TV_GameInstancesView";
            this.TV_GameInstancesView.SelectedImageIndex = 0;
            this.TV_GameInstancesView.Size = new System.Drawing.Size(199, 390);
            this.TV_GameInstancesView.TabIndex = 2;
            // 
            // IL_TreeView
            // 
            this.IL_TreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL_TreeView.ImageStream")));
            this.IL_TreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.IL_TreeView.Images.SetKeyName(0, "Instance");
            this.IL_TreeView.Images.SetKeyName(1, "Joueur");
            // 
            // BT_KillAllGames
            // 
            this.BT_KillAllGames.BackColor = System.Drawing.Color.Transparent;
            this.BT_KillAllGames.BackgroundImage = global::BattleshipServeur.Properties.Resources.BigRedButton;
            this.BT_KillAllGames.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BT_KillAllGames.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_KillAllGames.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_KillAllGames.Location = new System.Drawing.Point(318, 12);
            this.BT_KillAllGames.Name = "BT_KillAllGames";
            this.BT_KillAllGames.Size = new System.Drawing.Size(153, 150);
            this.BT_KillAllGames.TabIndex = 3;
            this.BT_KillAllGames.Text = "Tuer toutes les instances de Jeux";
            this.BT_KillAllGames.UseVisualStyleBackColor = false;
            this.BT_KillAllGames.Click += new System.EventHandler(this.BT_KillAllGames_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 390);
            this.Controls.Add(this.BT_KillAllGames);
            this.Controls.Add(this.TV_GameInstancesView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RB_Demarrer;
        private System.Windows.Forms.RadioButton RB_Arreter;
        private System.Windows.Forms.TreeView TV_GameInstancesView;
        private System.Windows.Forms.ImageList IL_TreeView;
        private System.Windows.Forms.Button BT_KillAllGames;
    }
}

