using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Gameplay.Board;
using _Project.Scripts.Gameplay.Players;
using _Project.Scripts.Gameplay.WinConditions;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Elements;
using _Project.Scripts.UI.History;
using UnityEngine;

namespace _Project.Scripts.Core.Services.Config
{
    public class ConfigService : IService
    {
        private Dictionary<PlayerType, PlayerConfig> _players;
        public GameScreen GameScreenPrefab { get; private set; }
        public WinLineView WinLinePrefab { get; private set; }
        public BoardConfig Board { get; private set; }
        public MoveHistoryElement MoveHistoryElementPrefab { get; private set; }
        public ClearBoardButton ClearBoardButtonPrefab { get; private set; }
        public RevertMoveButton RevertRevertMoveButtonPrefab { get; private set; }

        public void Initialize()
        { 
            LoadBoard();
            LoadPlayers();
            LoadGameScreen();
            LoadWinLine();
            LoadNumberedElement();
            LoadEmptyBoardButton();
            LoadRevertMoveButton();
        }

        private void LoadRevertMoveButton() => 
            RevertRevertMoveButtonPrefab = Resources.Load<RevertMoveButton>("RevertMove_btn");

        private void LoadEmptyBoardButton() => 
            ClearBoardButtonPrefab = Resources.Load<ClearBoardButton>("ClearBoard_btn");

        private void LoadNumberedElement() => 
            MoveHistoryElementPrefab = Resources.Load<MoveHistoryElement>("MoveHistoryElement");

        public TConfig GetPlayer<TConfig>(PlayerType type) where TConfig : PlayerConfig =>
            _players[type] as TConfig;

        private void LoadPlayers()
        {
            PlayerConfig[] configs = Resources.LoadAll<PlayerConfig>("Configs/Players");
            _players = configs.ToDictionary(cfg => cfg.TypeID, cfg => cfg);
        }

        private void LoadGameScreen() =>
            GameScreenPrefab = Resources.Load<GameScreen>("GameScreen");

        private void LoadBoard() =>
            Board = Resources.Load<BoardConfig>("Configs/BoardConfig");

        private void LoadWinLine() => 
            WinLinePrefab = Resources.Load<WinLineView>("WinLine");
    }
}