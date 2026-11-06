using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BattleshipGame.Models
{
    public class GameBoard
    {
        public int Size { get; }
        public CellState[,] Grid { get; }
        public List<Ship> Ships { get; }

        public GameBoard(int size)
        {
            Size = size;
            Grid = new CellState[size, size];
            Ships = new List<Ship>();
        }

        public bool CanPlaceShip(Ship ship)
        {
            var points = ship.GetOccupiedPoints();
            foreach (var p in points)
            {
                if (p.X < 0 || p.X >= Size || p.Y < 0 || p.Y >= Size)
                    return false;
                if (Grid[p.X, p.Y] != CellState.Water)
                    return false;
            }
            return true;
        }

        public bool PlaceShip(Ship ship)
        {
            if (!CanPlaceShip(ship)) return false;

            Ships.Add(ship);
            var points = ship.GetOccupiedPoints();
            foreach (var p in points)
            {
                Grid[p.X, p.Y] = CellState.Ship;
            }
            return true;
        }

        public (CellState result, Ship sunkShip) TakeShot(int x, int y)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
                return (CellState.Miss, null);

            if (Grid[x, y] == CellState.Hit || Grid[x, y] == CellState.Miss || Grid[x, y] == CellState.Sunk)
                return (Grid[x, y], null);

            if (Grid[x, y] == CellState.Ship)
            {
                Grid[x, y] = CellState.Hit;
                var ship = Ships.FirstOrDefault(s => s.GetOccupiedPoints().Contains(new Point(x, y)));
                if (ship != null)
                {
                    ship.Hits++;
                    if (ship.IsSunk)
                    {
                        foreach (var p in ship.GetOccupiedPoints())
                        {
                            Grid[p.X, p.Y] = CellState.Sunk;
                        }
                        return (CellState.Sunk, ship);
                    }
                }
                return (CellState.Hit, null);
            }
            else
            {
                Grid[x, y] = CellState.Miss;
                return (CellState.Miss, null);
            }
        }

        public bool AllShipsSunk()
        {
            return Ships.All(s => s.IsSunk);
        }

        public void AutoPlaceShips(List<ShipType> shipTypes)
        {
            Random rnd = new Random();
            foreach (var type in shipTypes)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = rnd.Next(Size);
                    int y = rnd.Next(Size);
                    ShipOrientation orientation = (ShipOrientation)rnd.Next(2);
                    Ship ship = new Ship { Type = type, X = x, Y = y, Orientation = orientation };
                    if (PlaceShip(ship)) placed = true;
                }
            }
        }
    }
}
