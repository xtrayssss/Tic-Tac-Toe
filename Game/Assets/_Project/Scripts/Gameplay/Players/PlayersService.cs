using System;
using System.Collections;
using System.Threading;
using _Project.Scripts.Core.Services.Config;
using _Project.Scripts.Core.Services.Input;
using _Project.Scripts.Gameplay.Board;
using _Project.Scripts.Gameplay.Players.AI;
using _Project.Scripts.Gameplay.Players.Human;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Gameplay.Players
{
    public class PlayersService : IPlayersService
    {
        private readonly IInputService _inputService;
        private readonly ConfigService _configService;
        public Player CurrentPlayer { get; set; }
        public BitArray PlayerBoard => CurrentPlayer.Board;
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public PlayersService(
            IInputService inputService,
            ConfigService configService)
        {
            _inputService = inputService;
            _configService = configService;
        }

        public Board<Cell> Board => CurrentPlayer.BoardGenerator.Board;

        public Player SwitchPlayer() =>
            CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;

        public async UniTask<int> PlayerMoveAsync(CancellationToken token) => 
            await CurrentPlayer.GetMoveAsync(token);

        public void SetMove(int move) => 
            CurrentPlayer.SetMove(move);

        public Player CreatePlayer(PlayerType type, BoardGenerator boardGenerator)
        {
            return type switch
            {
                PlayerType.HUMAN => CreateHumanPlayer(type, boardGenerator),
                PlayerType.RANDOM => CreateRandomPlayer(type, boardGenerator),
                PlayerType.AI => CreateAIPlayer(type, boardGenerator),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void SwapSymbols() => 
            (Player1.SymbolSettings, Player2.SymbolSettings) = (Player2.SymbolSettings, Player1.SymbolSettings);

        private Player CreateAIPlayer(PlayerType type, BoardGenerator boardGenerator)
        {
            AIPlayerConfig config = _configService.GetPlayer<AIPlayerConfig>(type);
            YandexAIPlayer aiPlayer = new YandexAIPlayer(config, boardGenerator);

            return aiPlayer;
        }

        private Player CreateHumanPlayer(PlayerType playerType, BoardGenerator boardGenerator)
        {
            PlayerConfig config = _configService.GetPlayer<PlayerConfig>(playerType);
            HumanPlayer player = new HumanPlayer(config, _inputService, boardGenerator);

            return player;
        }

        private Player CreateRandomPlayer(PlayerType playerType, BoardGenerator boardGenerator)
        {
            RandomPlayerConfig config = _configService.GetPlayer<RandomPlayerConfig>(playerType);
            RandomPlayer player = new RandomPlayer(config, boardGenerator);

            return player;
        }
    }
}