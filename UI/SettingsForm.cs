using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BattleshipGame.Models;
using BattleshipGame.AI;

namespace BattleshipGame.UI
{
    public partial class SettingsForm : Form
    {
        public int BoardSize { get; private set; } = 10;
        public AILevel Difficulty { get; private set; } = AILevel.Normal;
        public int ShipCount { get; private set; } = 5;

        private NumericUpDown numSize;
        private NumericUpDown numShips;
        private ComboBox comboDifficulty;
        private Button btnOK;

        public SettingsForm()
        {
            InitializeComponent();
            SetupControls();
        }

        private void SetupControls()
        {
            this.Text = "Game Settings";
            this.Size = new System.Drawing.Size(300, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblSize = new Label { Text = "Board Size (10-20):", Left = 20, Top = 20, Width = 120 };
            numSize = new NumericUpDown { Left = 150, Top = 18, Minimum = 10, Maximum = 20, Value = 10 };

            Label lblShips = new Label { Text = "Ship Count (1-8):", Left = 20, Top = 60, Width = 120 };
            numShips = new NumericUpDown { Left = 150, Top = 58, Minimum = 1, Maximum = 8, Value = 5 };

            Label lblDiff = new Label { Text = "CPU Difficulty:", Left = 20, Top = 100, Width = 120 };
            comboDifficulty = new ComboBox { Left = 150, Top = 98, DropDownStyle = ComboBoxStyle.DropDownList };
            comboDifficulty.Items.AddRange(Enum.GetNames(typeof(AILevel)));
            comboDifficulty.SelectedIndex = 2; // Normal

            btnOK = new Button { Text = "OK", Left = 100, Top = 150, DialogResult = DialogResult.OK };
            btnOK.Click += (s, e) => {
                BoardSize = (int)numSize.Value;
                ShipCount = (int)numShips.Value;
                Difficulty = (AILevel)comboDifficulty.SelectedIndex;
                this.Close();
            };

            this.Controls.Add(lblSize);
            this.Controls.Add(numSize);
            this.Controls.Add(lblShips);
            this.Controls.Add(numShips);
            this.Controls.Add(lblDiff);
            this.Controls.Add(comboDifficulty);
            this.Controls.Add(btnOK);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Name = "SettingsForm";
            this.ResumeLayout(false);
        }
    }
}
