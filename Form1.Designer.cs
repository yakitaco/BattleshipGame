namespace BattleshipGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.playerBoardControl = new BattleshipGame.UI.ShipBoardControl();
            this.cpuBoardControl = new BattleshipGame.UI.ShipBoardControl();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.lblPlayer = new System.Windows.Forms.Label();
            this.lblCPU = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playerBoardControl
            // 
            this.playerBoardControl.Location = new System.Drawing.Point(20, 50);
            this.playerBoardControl.Name = "playerBoardControl";
            this.playerBoardControl.Size = new System.Drawing.Size(350, 350);
            this.playerBoardControl.TabIndex = 0;
            this.playerBoardControl.Text = "shipBoardControl1";
            // 
            // cpuBoardControl
            // 
            this.cpuBoardControl.Location = new System.Drawing.Point(410, 50);
            this.cpuBoardControl.Name = "cpuBoardControl";
            this.cpuBoardControl.Size = new System.Drawing.Size(350, 350);
            this.cpuBoardControl.TabIndex = 1;
            this.cpuBoardControl.Text = "shipBoardControl2";
            this.cpuBoardControl.ShowShips = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.Location = new System.Drawing.Point(20, 420);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(200, 21);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Welcome to Battleship!";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(20, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "New Game";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(130, 10);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 30);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // lblPlayer
            // 
            this.lblPlayer.AutoSize = true;
            this.lblPlayer.Location = new System.Drawing.Point(20, 405);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new System.Drawing.Size(64, 15);
            this.lblPlayer.TabIndex = 5;
            this.lblPlayer.Text = "Your Board";
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.Location = new System.Drawing.Point(410, 405);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(65, 15);
            this.lblCPU.TabIndex = 6;
            this.lblCPU.Text = "CPU Board";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.lblCPU);
            this.Controls.Add(this.lblPlayer);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cpuBoardControl);
            this.Controls.Add(this.playerBoardControl);
            this.Name = "Form1";
            this.Text = "Battleship Game";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private BattleshipGame.UI.ShipBoardControl playerBoardControl;
        private BattleshipGame.UI.ShipBoardControl cpuBoardControl;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label lblPlayer;
        private System.Windows.Forms.Label lblCPU;

        #endregion
    }
}
