using System;
using System.Collections.Generic;
using BattleshipGame.Models;
using BattleshipGame.AI;

namespace BattleshipGame.Models
{
    public enum GameState
    {
        Setup,
        PlayerTurn,
        CPUTurn,
        GameOver
    }

    public class GameSession
    {
        public GameBoard PlayerBoard { get; private set; }
        public GameBoard CPUBoard { get; private set; }
        public GameState State { get; set; }
        public IBattleshipAI CPUAI { get; private set; }
        public int BoardSize { get; }
        public List<ShipType> ShipTypes { get; }

        public GameSession(int boardSize, List<ShipType> shipTypes, AILevel aiLevel)
        {
            BoardSize = boardSize;
            ShipTypes = shipTypes;
            PlayerBoard = new GameBoard(boardSize);
            CPUBoard = new GameBoard(boardSize);
            CPUAI = AIFactory.CreateAI(aiLevel, boardSize, shipTypes);
            State = GameState.Setup;
        }

        public void StartGame()
        {
            State = GameState.PlayerTurn;
        }

        public bool IsPlayerWin() => CPUBoard.AllShipsSunk();
        public bool IsCPUWin() => PlayerBoard.AllShipsSunk();
    }
}
