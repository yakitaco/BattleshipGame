using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BattleshipGame.Models;

namespace BattleshipGame.UI
{
    public class ShipBoardControl : Control
    {
        private GameBoard _board;
        public GameBoard Board
        {
            get => _board;
            set { _board = value; Invalidate(); }
        }

        public bool ShowShips { get; set; } = true;
        public bool Interactive { get; set; } = true;

        public event EventHandler<Point> CellClicked;

        public ShipBoardControl()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_board == null) return;

            Graphics g = e.Graphics;
            float cellWidth = (float)Width / _board.Size;
            float cellHeight = (float)Height / _board.Size;

            // Draw Grid
            for (int i = 0; i <= _board.Size; i++)
            {
                g.DrawLine(Pens.LightBlue, i * cellWidth, 0, i * cellWidth, Height);
                g.DrawLine(Pens.LightBlue, 0, i * cellHeight, Width, i * cellHeight);
            }

            // Draw Board content
            for (int x = 0; x < _board.Size; x++)
            {
                for (int y = 0; y < _board.Size; y++)
                {
                    RectangleF rect = new RectangleF(x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                    rect.Inflate(-1, -1);

                    var state = _board.Grid[x, y];
                    switch (state)
                    {
                        case CellState.Ship:
                            if (ShowShips) g.FillRectangle(Brushes.Gray, rect);
                            break;
                        case CellState.Hit:
                            g.FillRectangle(Brushes.Red, rect);
                            break;
                        case CellState.Miss:
                            g.FillEllipse(Brushes.LightGray, rect);
                            break;
                        case CellState.Sunk:
                            g.FillRectangle(Brushes.DarkRed, rect);
                            break;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!Interactive || _board == null) return;

            int x = (int)(e.X / ((float)Width / _board.Size));
            int y = (int)(e.Y / ((float)Height / _board.Size));

            if (x >= 0 && x < _board.Size && y >= 0 && y < _board.Size)
            {
                CellClicked?.Invoke(this, new Point(x, y));
                Invalidate();
            }
        }
    }
}
