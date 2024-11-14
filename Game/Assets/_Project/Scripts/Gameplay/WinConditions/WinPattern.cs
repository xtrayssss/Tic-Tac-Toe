using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Services;
using _Project.Scripts.Gameplay.Board;

namespace _Project.Scripts.Gameplay.WinConditions
{
    public class WinPattern : IService
    {
        private readonly List<BitArray> _winningPatterns;
        private readonly int _boardSize;

        private readonly List<int[]> _patternCells;
        private int[] _winningCells;

        public WinPattern(int boardSize)
        {
            _boardSize = boardSize;
            _winningPatterns = new List<BitArray>();
            _patternCells = new List<int[]>();
            GenerateWinningPatterns();
        }

        private void GenerateWinningPatterns()
        {
            for (int row = 0; row < _boardSize; row++)
            {
                BitArray pattern = new BitArray(_boardSize * _boardSize);
                int[] cells = new int[_boardSize];

                for (int col = 0; col < _boardSize; col++)
                {
                    int index = row * _boardSize + col;
                    pattern[index] = true;
                    cells[col] = index;
                }

                _winningPatterns.Add(pattern);
                _patternCells.Add(cells);
            }

            for (int col = 0; col < _boardSize; col++)
            {
                BitArray pattern = new BitArray(_boardSize * _boardSize);
                int[] cells = new int[_boardSize];

                for (int row = 0; row < _boardSize; row++)
                {
                    int index = row * _boardSize + col;
                    pattern[index] = true;
                    cells[row] = index;
                }

                _winningPatterns.Add(pattern);
                _patternCells.Add(cells);
            }

            {
                BitArray pattern = new BitArray(_boardSize * _boardSize);
                int[] cells = new int[_boardSize];

                for (int i = 0; i < _boardSize; i++)
                {
                    int index = i * _boardSize + i;
                    pattern[index] = true;
                    cells[i] = index;
                }

                _winningPatterns.Add(pattern);
                _patternCells.Add(cells);
            }

            {
                BitArray pattern = new BitArray(_boardSize * _boardSize);
                int[] cells = new int[_boardSize];

                for (int i = 0; i < _boardSize; i++)
                {
                    int index = i * _boardSize + (_boardSize - 1 - i);
                    pattern[index] = true;
                    cells[i] = index;
                }

                _winningPatterns.Add(pattern);
                _patternCells.Add(cells);
            }
        }

        public bool CheckWin(BitArray board)
        {
            for (int i = 0; i < _winningPatterns.Count; i++)
            {
                BitArray pattern = _winningPatterns[i];
                BitArray temp = new BitArray(board);

                temp.And(pattern);
                bool isMatch = true;

                for (int j = 0; j < temp.Length; j++)
                {
                    if (pattern[j] && !temp[j])
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    _winningCells = _patternCells[i];

                    return true;
                }
            }

            return false;
        }

        public WinLineInfo GetWineLine(Board<Cell> board) => 
            CreateWinLineInfo(_winningCells, board);

        private static WinLineInfo CreateWinLineInfo(int[] cells, Board<Cell> board)
        {
            return new WinLineInfo
            {
                StartPosition = board.Cells[cells[0]].CellView.RectTransform.anchoredPosition,
                EndPosition = board.Cells[cells[cells.Length - 1]].CellView.RectTransform.anchoredPosition
            };
        }
    }
}