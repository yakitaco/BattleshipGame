using BattleshipGame.Models;
using BattleshipGame.AI;
using BattleshipGame.UI;
using System.Linq;

namespace BattleshipGame
{
    public partial class Form1 : Form
    {
        private GameSession _session;
        private int _boardSize = 10;
        private int _shipCount = 5;
        private AILevel _difficulty = AILevel.Normal;

        public Form1()
        {
            InitializeComponent();
            cpuBoardControl.CellClicked += CpuBoardControl_CellClicked;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SetupGame();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settings = new SettingsForm())
            {
                if (settings.ShowDialog() == DialogResult.OK)
                {
                    _boardSize = settings.BoardSize;
                    _difficulty = settings.Difficulty;
                    _shipCount = settings.ShipCount;
                    lblStatus.Text = "Settings updated. Start a new game!";
                }
            }
        }

        private void SetupGame()
        {
            var allShipTypes = ShipType.GetDefaultShips();
            var shipTypes = allShipTypes.Take(_shipCount).ToList();
            
            _session = new GameSession(_boardSize, shipTypes, _difficulty);
            
            _session.PlayerBoard.AutoPlaceShips(shipTypes);
            _session.CPUBoard.AutoPlaceShips(shipTypes);

            playerBoardControl.Board = _session.PlayerBoard;
            cpuBoardControl.Board = _session.CPUBoard;

            _session.StartGame();
            lblStatus.Text = "Game Started! Your turn.";
            cpuBoardControl.Enabled = true;
            cpuBoardControl.Invalidate();
            playerBoardControl.Invalidate();
        }

        private async void CpuBoardControl_CellClicked(object sender, Point p)
        {
            if (_session == null || _session.State != GameState.PlayerTurn) return;

            var result = _session.CPUBoard.TakeShot(p.X, p.Y);
            if (result.result == CellState.Hit || result.result == CellState.Miss || result.result == CellState.Sunk)
            {
                cpuBoardControl.Invalidate();
                if (result.sunkShip != null)
                    lblStatus.Text = $"You sunk their {result.sunkShip.Type.Name}!";
                else
                    lblStatus.Text = result.result == CellState.Hit ? "Hit!" : "Miss!";

                if (_session.IsPlayerWin())
                {
                    lblStatus.Text = "VICTORY! You won!";
                    _session.State = GameState.GameOver;
                    cpuBoardControl.Enabled = false;
                    return;
                }

                // CPU Turn
                _session.State = GameState.CPUTurn;
                cpuBoardControl.Enabled = false;
                await Task.Delay(1000); // Wait for "thinking"
                ExecuteCPUTurn();
            }
        }

        private void ExecuteCPUTurn()
        {
            Point shot = _session.CPUAI.GetNextShot(_session.PlayerBoard);
            if (shot.X != -1)
            {
                var result = _session.PlayerBoard.TakeShot(shot.X, shot.Y);
                playerBoardControl.Invalidate();

                if (result.sunkShip != null)
                    lblStatus.Text = $"CPU sunk your {result.sunkShip.Type.Name}!";
                else
                    lblStatus.Text = result.result == CellState.Hit ? "CPU Hit!" : "CPU Missed!";

                if (_session.IsCPUWin())
                {
                    lblStatus.Text = "DEFEAT! CPU won!";
                    _session.State = GameState.GameOver;
                    return;
                }

                _session.State = GameState.PlayerTurn;
                cpuBoardControl.Enabled = true;
            }
        }
    }
}
