using System;
using System.Collections.Generic;
using System.Drawing;

namespace BattleshipGame.Models
{
    public enum CellState
    {
        Water,
        Ship,
        Hit,
        Miss,
        Sunk
    }

    public enum ShipOrientation
    {
        Horizontal,
        Vertical
    }

    public class ShipType
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public Color Color { get; set; }

        public ShipType(string name, int size, Color color)
        {
            Name = name;
            Size = size;
            Color = color;
        }

        public static List<ShipType> GetDefaultShips()
        {
            return new List<ShipType>
            {
                new ShipType("Carrier", 5, Color.DarkSlateGray),
                new ShipType("Battleship", 4, Color.SlateGray),
                new ShipType("Destroyer", 3, Color.Gray),
                new ShipType("Submarine", 3, Color.DimGray),
                new ShipType("Patrol Boat", 2, Color.LightSlateGray),
                new ShipType("Corvette", 2, Color.Silver),
                new ShipType("Frigate", 3, Color.LightGray),
                new ShipType("Cruiser", 4, Color.DarkGray)
            };
        }
    }

    public class Ship
    {
        public ShipType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ShipOrientation Orientation { get; set; }
        public int Hits { get; set; }
        public bool IsSunk => Hits >= Type.Size;

        public List<Point> GetOccupiedPoints()
        {
            var points = new List<Point>();
            for (int i = 0; i < Type.Size; i++)
            {
                if (Orientation == ShipOrientation.Horizontal)
                    points.Add(new Point(X + i, Y));
                else
                    points.Add(new Point(X, Y + i));
            }
            return points;
        }
    }
}
