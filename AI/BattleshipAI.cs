using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BattleshipGame.Models;

namespace BattleshipGame.AI
{
    public interface IBattleshipAI
    {
        Point GetNextShot(GameBoard opponentBoard);
    }

    public enum AILevel
    {
        Novice,    // Random
        Easy,      // Random + Target Mode
        Normal,    // Parity + Target Mode
        Hard,      // Probability Map
        Hardcore   // Optimized Probability Map
    }

    public class AIFactory
    {
        public static IBattleshipAI CreateAI(AILevel level, int boardSize, List<ShipType> shipTypes)
        {
            switch (level)
            {
                case AILevel.Novice: return new NoviceAI(boardSize);
                case AILevel.Easy: return new EasyAI(boardSize);
                case AILevel.Normal: return new NormalAI(boardSize);
                case AILevel.Hard: return new ProbabilityMapAI(boardSize, shipTypes, false);
                case AILevel.Hardcore: return new ProbabilityMapAI(boardSize, shipTypes, true);
                default: return new NoviceAI(boardSize);
            }
        }
    }

    // Level 1: Novice - Purely Random
    public class NoviceAI : IBattleshipAI
    {
        protected Random _rnd = new Random();
        protected int _size;

        public NoviceAI(int size) { _size = size; }

        public virtual Point GetNextShot(GameBoard opponentBoard)
        {
            List<Point> possibleShots = new List<Point>();
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (opponentBoard.Grid[x, y] == CellState.Water || opponentBoard.Grid[x, y] == CellState.Ship)
                        possibleShots.Add(new Point(x, y));
                }
            }
            if (possibleShots.Count == 0) return new Point(-1, -1);
            return possibleShots[_rnd.Next(possibleShots.Count)];
        }
    }

    // Level 2: Easy - Random + Hunt/Target
    public class EasyAI : NoviceAI
    {
        public EasyAI(int size) : base(size) { }

        public override Point GetNextShot(GameBoard opponentBoard)
        {
            // Look for existing hits that aren't sunk
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (opponentBoard.Grid[x, y] == CellState.Hit)
                    {
                        var target = GetTargetShot(opponentBoard, x, y);
                        if (target.X != -1) return target;
                    }
                }
            }
            return base.GetNextShot(opponentBoard);
        }

        protected Point GetTargetShot(GameBoard board, int x, int y)
        {
            Point[] adj = { new Point(x + 1, y), new Point(x - 1, y), new Point(x, y + 1), new Point(x, y - 1) };
            foreach (var p in adj)
            {
                if (p.X >= 0 && p.X < _size && p.Y >= 0 && p.Y < _size)
                {
                    if (board.Grid[p.X, p.Y] == CellState.Water || board.Grid[p.X, p.Y] == CellState.Ship)
                        return p;
                }
            }
            return new Point(-1, -1);
        }
    }

    // Level 3: Normal - Parity + Target
    public class NormalAI : EasyAI
    {
        public NormalAI(int size) : base(size) { }

        public override Point GetNextShot(GameBoard opponentBoard)
        {
            // First check Target mode
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (opponentBoard.Grid[x, y] == CellState.Hit)
                    {
                        var target = GetTargetShot(opponentBoard, x, y);
                        if (target.X != -1) return target;
                    }
                }
            }

            // Hunting mode with Parity (Checkerboard)
            List<Point> parityShots = new List<Point>();
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if ((x + y) % 2 == 0)
                    {
                        if (opponentBoard.Grid[x, y] == CellState.Water || opponentBoard.Grid[x, y] == CellState.Ship)
                            parityShots.Add(new Point(x, y));
                    }
                }
            }

            if (parityShots.Count > 0)
                return parityShots[_rnd.Next(parityShots.Count)];

            return base.GetNextShot(opponentBoard);
        }
    }

    // Level 4 & 5: Probability Map AI
    public class ProbabilityMapAI : IBattleshipAI
    {
        private int _size;
        private List<ShipType> _shipTypes;
        private bool _isHardcore;
        private Random _rnd = new Random();

        public ProbabilityMapAI(int size, List<ShipType> shipTypes, bool isHardcore)
        {
            _size = size;
            _shipTypes = shipTypes;
            _isHardcore = isHardcore;
        }

        public Point GetNextShot(GameBoard opponentBoard)
        {
            // Target mode takes precedence if there are hits
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (opponentBoard.Grid[x, y] == CellState.Hit)
                    {
                        var target = GetTargetShot(opponentBoard, x, y);
                        if (target.X != -1) return target;
                    }
                }
            }

            // Hunting mode: Probability Heatmap
            int[,] heatmap = new int[_size, _size];
            var aliveShips = _shipTypes.Where(t => !opponentBoard.Ships.Any(s => s.Type.Name == t.Name && s.IsSunk)).ToList();

            foreach (var shipType in aliveShips)
            {
                for (int x = 0; x < _size; x++)
                {
                    for (int y = 0; y < _size; y++)
                    {
                        // Check horizontal
                        if (CanFit(opponentBoard, x, y, shipType.Size, Orientation.Horizontal))
                        {
                            for (int i = 0; i < shipType.Size; i++) heatmap[x + i, y]++;
                        }
                        // Check vertical
                        if (CanFit(opponentBoard, x, y, shipType.Size, Orientation.Vertical))
                        {
                            for (int i = 0; i < shipType.Size; i++) heatmap[x, y + i]++;
                        }
                    }
                }
            }

            // Hardcore tweaks: avoid edges if ships are likely in middle, or favor specific patterns
            if (_isHardcore)
            {
                // In Hardcore, we might add a small weight to the center or avoid "dead zones"
            }

            int maxVal = -1;
            List<Point> bestShots = new List<Point>();

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (opponentBoard.Grid[x, y] == CellState.Water || opponentBoard.Grid[x, y] == CellState.Ship)
                    {
                        if (heatmap[x, y] > maxVal)
                        {
                            maxVal = heatmap[x, y];
                            bestShots.Clear();
                            bestShots.Add(new Point(x, y));
                        }
                        else if (heatmap[x, y] == maxVal)
                        {
                            bestShots.Add(new Point(x, y));
                        }
                    }
                }
            }

            if (bestShots.Count == 0) return new Point(-1, -1);
            return bestShots[_rnd.Next(bestShots.Count)];
        }

        private bool CanFit(GameBoard board, int x, int y, int size, Orientation orientation)
        {
            for (int i = 0; i < size; i++)
            {
                int nx = x + (orientation == Orientation.Horizontal ? i : 0);
                int ny = y + (orientation == Orientation.Vertical ? i : 0);

                if (nx < 0 || nx >= _size || ny < 0 || ny >= _size) return false;
                
                // If we know it's a Miss or Sunk, we can't fit a ship here
                if (board.Grid[nx, ny] == CellState.Miss || board.Grid[nx, ny] == CellState.Sunk) return false;
                
                // Note: We count 'Hit' as potentially fitting because it COULD be part of the ship we are looking for,
                // but the GetTargetShot logic usually handles that first.
            }
            return true;
        }

        protected Point GetTargetShot(GameBoard board, int x, int y)
        {
            Point[] adj = { new Point(x + 1, y), new Point(x - 1, y), new Point(x, y + 1), new Point(x, y - 1) };
            foreach (var p in adj)
            {
                if (p.X >= 0 && p.X < _size && p.Y >= 0 && p.Y < _size)
                {
                    if (board.Grid[p.X, p.Y] == CellState.Water || board.Grid[p.X, p.Y] == CellState.Ship)
                        return p;
                }
            }
            return new Point(-1, -1);
        }
    }
}
