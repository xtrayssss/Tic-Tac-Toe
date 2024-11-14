using System.Collections;
using System.Collections.Generic;
using System.Text;
using _Project.Scripts.Core.Services;
using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Board
{
    public class Board<TCell> : IService where TCell : ICell
    {
        private float CellSize => Config.CellSize;
        private float Offset => Config.Offset;
        public CellView CellPrefab => Config.CellPrefab;
        public int Size => Config.Size;
        private BoardConfig Config { get; }

        public readonly TCell[] Cells;

        public readonly RectTransform Container;
        private readonly BitArray _board;

        public Board(BoardConfig config, RectTransform container)
        {
            Config = config;
            Container = container;

            _board = new BitArray(length: Config.Size * Config.Size);

            Cells = new TCell[Config.Size * Config.Size];


#if UNITY_EDITOR
            DisplayBoard();
#endif
        }

        public float2 GetUIPosition(float2 position)
        {
            float x = position.x * (CellSize + Offset);
            float y = position.y * (CellSize + Offset);

            return new float2(x, y);
        }

        public void ClearBoard() =>
            _board.SetAll(false);

        public int2 GetCellPosition(float2 position, RectTransform container)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(container, position, Camera.main,
                out Vector2 uiPosition);

            float cellAndOffset = Config.CellSize + Config.Offset;

            int cellX = (int)(uiPosition.x / cellAndOffset);
            int cellY = (int)(uiPosition.y / cellAndOffset);

            float cellStartX = cellX * cellAndOffset;
            float cellStartY = cellY * cellAndOffset;

            bool isInCellX = uiPosition.x >= cellStartX &&
                             uiPosition.x < cellStartX + Config.CellSize;

            bool isInCellY = uiPosition.y >= cellStartY &&
                             uiPosition.y < cellStartY + Config.CellSize;

            if (!isInCellX || !isInCellY)
            {
                return new int2(-1, -1);
            }

            return new int2(cellX, cellY);
        }

        public bool IsWithinBoard(int2 position)
        {
            return position.x >= 0 && position.x < Size &&
                   position.y >= 0 && position.y < Size;
        }

        public void SetMove(int bitIndex, BitArray board)
        {
            _board[bitIndex] = true;
            board[bitIndex] = true;

#if UNITY_EDITOR
            DisplayBoard();
#endif
        }

        public void ResetMove(int bitIndex, BitArray board)
        {
            _board[bitIndex] = false;
            board[bitIndex] = false;

#if UNITY_EDITOR
            DisplayBoard();
#endif
        }

        public bool IsOccupied(int2 position) =>
            _board[CoordsToBitIndex(position.x, position.y)];

        public bool IsFull()
        {
            for (int i = 0; i < Size * Size; i++)
            {
                if (!_board[i])
                    return false;
            }

            return true;
        }

        public List<int> GetFreeCells()
        {
            List<int> freeCells = new List<int>();

            for (int i = 0; i < Size * Size; i++)
            {
                if (!_board[i])
                    freeCells.Add(i);
            }

            return freeCells;
        }

        private void DisplayBoard() =>
            Debug.Log("Current board state:\n" + ConvertBitboardToString());

        public string ConvertBitboardToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int col = Config.Size - 1; col >= 0; col--)
            {
                for (int row = 0; row < Config.Size; row++)
                {
                    int bitIndex = CoordsToBitIndex(row, col);
                    char symbol = '_';

                    if (_board[bitIndex])
                    {
                        symbol = Cells[bitIndex].Symbol;
                    }

                    sb.Append(symbol);

                    if (row < Config.Size - 1)
                    {
                        sb.Append('|');
                    }
                }

                if (col > 0)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        public int CoordsToBitIndex(int row, int col) =>
            row * Config.Size + col;
    }
}