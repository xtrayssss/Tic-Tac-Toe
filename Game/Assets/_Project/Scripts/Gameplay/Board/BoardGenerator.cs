using _Project.Scripts.Core.Services;
using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Board
{
    public class BoardGenerator : IService
    {
        public BoardConfig Config { get; }
        public Board<Cell> Board { get; private set; }

        public BoardGenerator(
            BoardConfig config)
        {
            Config = config;
        }

        public Board<Cell> Generate(RectTransform board)
        {
            Board = new Board<Cell>(Config, board);

            float gridConfigCellSize = Config.Size * (Config.CellSize + Config.Offset) - Config.Offset;

            for (int x = 0; x < Config.Size; x++)
            {
                for (int y = 0; y < Config.Size; y++)
                {
                    float2 position = Board.GetUIPosition(new float2(x, y));

                    CellView cellView = Object.Instantiate(
                        original: Board.CellPrefab,
                        parent: board);

                    cellView.RectTransform.anchoredPosition =
                        position + new float2(Config.CellSize / 2, Config.CellSize / 2);

                    cellView.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Config.CellSize);
                    cellView.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Config.CellSize);

                    Board.Cells[x * Config.Size + y] = new Cell(cellView: cellView);
                }
            }

            board.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gridConfigCellSize);
            board.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gridConfigCellSize);

            return Board;
        }
    }
}